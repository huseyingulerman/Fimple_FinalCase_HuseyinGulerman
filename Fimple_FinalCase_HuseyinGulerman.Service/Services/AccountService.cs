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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class AccountService : Service<Account, AccountCreateDTO, AccountDTO>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<AppUser> _userService;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork uow, IMapper mapper, IAccountRepository accountRepository, UserManager<AppUser> userService) : base(uow, mapper)
        {
            _userService=userService;
            _accountRepository=accountRepository;
            _mapper=mapper;
        }

        /// <summary>
        /// Açılacak hesap bilgilerini ve hesabı açılan kullanıcının kimlik numarasını alarak hesap açılışını gerçekleştiriyor.
        /// </summary>
        /// <param name="accountCreateDTO"></param>
        /// <param name="identificationNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IAppResult<AccountDTO>> AddAccountAsync(AccountCreateDTO accountCreateDTO, string identificationNumber)
        {
            const short _minBalance = 10;

            //Kullanıcı ilk defa oluşturulurken vadesiz hesabı açılıyor. Diğer hesap açılışları yaparken bu kontrol yapılıyor.
            if (accountCreateDTO.AccountType==AccountType.DepositAccount)
            {
                throw new ArgumentException("Vadesiz hesap 1 taneden fazla açılamaz");
            }
            if (_minBalance>accountCreateDTO.AccountBalance )
            {
                throw new ArgumentException("Açılan hesap bakiyesi 10 TL den az olamaz");
            }
            if (!Enum.IsDefined(typeof(AccountType), accountCreateDTO.AccountType))
            {
                throw new ArgumentException("Bu hesap tipi kullanılamamaktadır.");
            }
            var accountCurrency = _accountRepository.GetAllAsync(x => x.AppUserId==accountCreateDTO.AppUserId, x => x.AccountType==AccountType.DepositAccount, x => x.IsActive==true).AsNoTracking();
            foreach (var item in accountCurrency)
            {
                if (item.AccountBalance<0)
                {
                    throw new ArgumentException("Hesap açılışı için yeterli bakiye bulunmamaktadır.");
                }
                item.AccountBalance-=accountCreateDTO.AccountBalance;
                var isItTrue = _accountRepository.Update(item);
            }

            var accountNumber = AccountNumberGenerator(identificationNumber);
            accountCreateDTO.AccountNumber = accountNumber;
            var account = _mapper.Map<Account>(accountCreateDTO);
            await _accountRepository.AddAsync(account);
            var isItCommit = _uow.saveChanges();
            var accountDTO = _mapper.Map<AccountDTO>(account);
            return AppResult<AccountDTO>.Success(StatusCodes.Status200OK, accountDTO);
        }

        /// <summary>
        /// Vadeli hesap açılışını yapıyor.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IAppResult<AccountDTO>> AddDepositAccountAsync(AppUser appUser)
        {
            var accountCurrency =await _accountRepository.GetAllAsync(x => x.AppUserId==appUser.Id, x => x.AccountType==AccountType.DepositAccount, x => x.IsActive==true).AsNoTracking().ToListAsync();
            foreach (var item in accountCurrency)
            {
                if (accountCurrency.Count()!=null)
                {
                    throw new ArgumentException("Vadesiz hesap 1 taneden fazla açılamaz");
                }
            }
            Account account = new Account($"{appUser.FirstName}{appUser.LastName}-Vadeli Hesabım", appUser.LastName, appUser.FirstName, appUser.Id);
            var accountNumber = AccountNumberGenerator(appUser.IdentificationNumber);
            account.AccountNumber = accountNumber;
            account.DailyTransactionLimit=2000;
            account.OneTimeTransactionLimit=1000;
            account.IsActive=true;
            await _accountRepository.AddAsync(account);
            var isItCommit = _uow.saveChanges();
            var accountDTO = _mapper.Map<AccountDTO>(account);
            return AppResult<AccountDTO>.Success(StatusCodes.Status200OK, accountDTO);
        }

        /// <summary>
        /// Sorguda istenilene göre hesap getiriyor.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Task<Account> GetAccountByExpAsync(Expression<Func<Account, bool>> exp)
        {
            var query = _accountRepository.GetAccountByExpAsync(exp);

            return query;
        }

        /// <summary>
        /// Kimlik numarasına göre uniq bir hesap numarası üretiyor.
        /// </summary>
        /// <param name="identificationNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string AccountNumberGenerator(string identificationNumber)
        {
            if (string.IsNullOrEmpty(identificationNumber) || identificationNumber.Length != 11)
            {

                throw new ArgumentException("Geçersiz TC Kimlik Numarası");
            }
            var _random = Random.Shared.Next(1000, 9999);
            string accountNumber = $"1000{_random}{identificationNumber.Substring(7, 4)}";

            return accountNumber;
        }
    }
}
