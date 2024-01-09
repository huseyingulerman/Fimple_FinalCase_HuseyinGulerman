using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Repositories;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Concrete;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Fimple_FinalCase_HuseyinGulerman.Repository.Migrations;
using Fimple_FinalCase_HuseyinGulerman.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class ProcessService : Service<Process, ProcessCreateDTO, ProcessDTO>, IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private static readonly object balanceLock = new object();
        public ProcessService(IUnitOfWork uow, IMapper mapper, IProcessRepository processRepository, IAccountRepository accountRepository) : base(uow, mapper)
        {
            _accountRepository=accountRepository;
            _processRepository=processRepository;
            _mapper=mapper;
            _uow=uow;
        }

        public async Task<IAppResult<ProcessDTO>> AddAutomaticAsync(AppUser appUser, ProcessAutomaticPaymentCreateDTO processAutomaticPaymentCreateDTO)
        {
            var _process = _mapper.Map<Process>(processAutomaticPaymentCreateDTO);
            _process.ProcessStatus=ProcessStatus.FuturePayment;
            _process.AppUserId=appUser.Id;
            _process.IsActive=true;
            _process.ProcessType=ProcessType.AutomaticPayment;
            _processRepository.AddAsync(_process);
            await _uow.CommitAsync();
            return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(_process));
        }

        public async Task<IAppResult<ProcessDTO>> AddDepositMoneyAsync(AppUser appUser, ProcessCreateDTO processCreateDTO, AccountDTO account)
        {

            const double _feeRequiringApproval = 50000;
            lock (balanceLock)
            {
                //limit kontrolü yapılıyor
                if (processCreateDTO.AmountSent>_feeRequiringApproval)
                {
                    processCreateDTO.ProcessStatus=ProcessStatus.WaitingForApproval;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    return AppResult<ProcessDTO>.Fail(StatusCodes.Status400BadRequest, "Bu işlem için yönetici izni gerekli.");
                }

                account.AccountBalance+=processCreateDTO.AmountSent;
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.OutgoingAccountNumber=account.AccountNumber;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(account));
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));

            }

        }

        public async Task<IAppResult<ProcessDTO>> MakeAutomaticPayment(ProcessCreateDTO CreateDTO)
        {
            lock (balanceLock)
            {  //Gönderici ve alıcının hesaplarını çağırır.
                var accountDTO = _accountRepository.GetAccountByExpAsync(x => x.Id==CreateDTO.AccountId);
                if (accountDTO.Result.AccountBalance<CreateDTO.AmountSent)
                {
                    throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.Otomatik ödeme yapılamamıştır.");
                }
                var outgoingAccountDTO =  _accountRepository.GetAccountByExpAsync(x => x.AccountNumber==CreateDTO.OutgoingAccountNumber);

                CreateDTO.ProcessStatus =ProcessStatus.Successful;
                CreateDTO.IsActive=true;
                _processRepository.Update(_mapper.Map<Process>(CreateDTO));
                //Gönderici ve alıcının hesaplarından para transferlerini yapar.
                outgoingAccountDTO.Result.AccountBalance+=CreateDTO.AmountSent;
                accountDTO.Result.AccountBalance-=CreateDTO.AmountSent;
                _accountRepository.Update(_mapper.Map<Account>(outgoingAccountDTO.Result));
                _accountRepository.Update(_mapper.Map<Account>(accountDTO.Result));
                //Otomatik ödeme için bir dahaki aya işlem oluşturur.
                var process = _mapper.Map<Process>(CreateDTO);
                process.PaymentDate=process.PaymentDate.Date.AddMonths(1);
                process.ProcessStatus =ProcessStatus.FuturePayment;
                process.Id=0;
                _processRepository.AddAsync(process);
                var a = _uow.saveChanges();
                var _processDTO = _mapper.Map<ProcessDTO>(process);
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _processDTO);
            }

        }
        //Otomatik ödemesi yapılan işlemi günceller
        public void UpdatesTransactionsInAutomaticPayment(ProcessCreateDTO processCreateDTO)
        {

            processCreateDTO.ProcessStatus =ProcessStatus.Successful;
            _processRepository.Update(_mapper.Map<Process>(processCreateDTO));

        }

        public async Task<IAppResult<ProcessDTO>> AddSendingMoneyAndPaymentAsync(AppUser appUser, ProcessCreateDTO processCreateDTO, AccountDTO account)
        {
            lock (balanceLock)
            {
                const double _feeRequiringApproval = 50000;
                //limit kontrolü yapılıyor
                if (processCreateDTO.AmountSent>_feeRequiringApproval)
                {
                    processCreateDTO.ProcessStatus=ProcessStatus.WaitingForApproval;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    _uow.saveChanges();
                    return AppResult<ProcessDTO>.Fail(StatusCodes.Status400BadRequest, "Bu işlem için yönetici izni gerekli.");
                }
                //Bakiye kontrolü yapılıyor
                if (processCreateDTO.AmountSent>account.AccountBalance)
                {
                    //return AppResult<AccountDTO>.Fail(StatusCodes.Status400BadRequest, "Hesabınızda yeterli bakiye bulunmamaktadır.");
                    processCreateDTO.ProcessStatus=ProcessStatus.unSuccessful;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    _uow.saveChanges();
                    throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.");
                }
                var isItTrue = IsTheDailyTransactionLimitExceeded(appUser, account);

                if (isItTrue)
                {
                    throw new Exception("Günlük işlem limitinizi aştınız.");
                }
                var isItTruee = IsTheOneTimeTransactionLimitExceeded(processCreateDTO, account);
                if (!isItTrue)
                {
                    throw new Exception("Tek seferlik işlem limitinizi aştınız.");
                }
                account.AccountBalance-=processCreateDTO.AmountSent;
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(account));
                var outgoingAccount = _accountRepository.GetAccountByExpAsync(x => x.AccountNumber==processCreateDTO.OutgoingAccountNumber);
                outgoingAccount.Result.AccountBalance+=processCreateDTO.AmountSent;
                var resultOutgoingAccountDTO = _accountRepository.Update(outgoingAccount.Result);
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
            }
        }
        private bool IsTheDailyTransactionLimitExceeded(AppUser appUser, AccountDTO account)
        {
            double _totalAmountSent = 0;
            var _process = _processRepository.GetAllAsync(x => x.AppUserId==appUser.Id, x => x.CreatedDate.Date==DateTime.UtcNow.Date, x => x.ProcessStatus==ProcessStatus.Successful, x => x.ProcessType!=ProcessType.DepozitMoney);

            foreach (var item in _process)
            {
                _totalAmountSent+=item.AmountSent;
            }
            if (_totalAmountSent>account.DailyTransactionLimit)
            {
                return true;
            }
            return false;
        }

        private bool IsTheOneTimeTransactionLimitExceeded(ProcessCreateDTO processCreateDTO, AccountDTO account)
        {
            if (processCreateDTO.AmountSent>account.OneTimeTransactionLimit)
            {
                return false;
            }
            return true;
        }

        public async Task<IAppResult<ProcessDTO>> AddWithdrawMoneyAsync(AppUser appUser, ProcessCreateDTO processCreateDTO, AccountDTO account)
        {
            lock (balanceLock)
            {
                const double _feeRequiringApproval = 50000;
                //limit kontrolü yapılıyor
                if (processCreateDTO.AmountSent>_feeRequiringApproval)
                {
                    processCreateDTO.ProcessStatus=ProcessStatus.WaitingForApproval;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    _uow.saveChanges();
                    return AppResult<ProcessDTO>.Fail(StatusCodes.Status400BadRequest, "Bu işlem için yönetici izni gerekli.");
                }
                //Bakiye kontrolü yapılıyor
                if (processCreateDTO.AmountSent>account.AccountBalance)
                {
                    //return AppResult<AccountDTO>.Fail(StatusCodes.Status400BadRequest, "Hesabınızda yeterli bakiye bulunmamaktadır.");
                    processCreateDTO.ProcessStatus=ProcessStatus.unSuccessful;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.");
                }
                var isItTrue = IsTheDailyTransactionLimitExceeded(appUser, account);

                if (isItTrue)
                {
                    throw new Exception("Günlük işlem limitinizi aştınız.");
                }
                var isItTruee = IsTheOneTimeTransactionLimitExceeded(processCreateDTO, account);
                if (isItTruee)
                {
                    throw new Exception("Tek seferlik işlem limitinizi aştınız.");
                }
                account.AccountBalance-=processCreateDTO.AmountSent;
                processCreateDTO.OutgoingAccountNumber="";
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(account));
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
            }
        }
    }
}