using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Repositories;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Concrete;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class AccountService : Service<Account, AccountCreateDTO, AccountDTO>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork uow, IMapper mapper, IAccountRepository accountRepository) : base(uow, mapper)
        {
            _accountRepository=accountRepository;
            _mapper=mapper;
        }

        public async Task<IAppResult<AccountDTO>> AddAccountAsync(AccountCreateDTO accountCreateDTO, string identificationNumber)
        {
 
            //Kullanıcı ilk defa oluşturulurken vadesiz hesabı açılıyor. Diğer hesap açılışları yaparken bu kontrol yapılıyor.
            if (accountCreateDTO.AccountType!=AccountType.DepositAccount)
            {
                var accountCurrency = _accountRepository.GetAllAsync(x => x.AppUserId==accountCreateDTO.AppUserId, x => x.AccountType==AccountType.DepositAccount, x => x.IsActive==true).AsNoTracking();
                foreach (var item in accountCurrency)
                {
                    item.AccountBalance-=accountCreateDTO.AccountBalance;
                    var isItTrue = _accountRepository.Update(item);
                    const short _minBalance = 10;
                    if (_minBalance>accountCreateDTO.AccountBalance || item.AccountBalance<0)
                    {
                        throw new ArgumentException("Açılan hesap bakiyesi 10 TL den az olamaz");
                    }
                }
            }
            var accountNumber = AccountNumberGenerator(identificationNumber);
            accountCreateDTO.AccountNumber = accountNumber;
            var account = _mapper.Map<Account>(accountCreateDTO);
          
            await _accountRepository.AddAsync(account);
        var isItCommit= _uow.saveChanges();
           var accountDTO= _mapper.Map<AccountDTO>(account);
            return AppResult<AccountDTO>.Success(StatusCodes.Status200OK, accountDTO);
       }

        public Task<Account> GetAccountByExpAsync(Expression<Func<Account, bool>> exp)
        {
            var query = _accountRepository.GetAccountByExpAsync(exp);

            return query;
        }

        private string AccountNumberGenerator(string identificationNumber)
        {
            if (string.IsNullOrEmpty(identificationNumber) || identificationNumber.Length != 11)
            {

                throw new ArgumentException("Geçersiz TC Kimlik Numarası");
            }
           var _random= Random.Shared.Next(1000, 9999);
            string accountNumber = $"1000{_random}{identificationNumber.Substring(7, 4)}";

            return accountNumber;
        }
    }
}
