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
using Fimple_FinalCase_HuseyinGulerman.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class ProcessService : Service<Process, ProcessCreateDTO, ProcessDTO>, IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

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

        public async Task<IAppResult<ProcessDTO>> AddProcessAsync(AppUser appUser, ProcessCreateDTO processCreateDTO, AccountDTO _account)
        {
            const double _feeRequiringApproval = 50000;

            //limit kontrolü yapılıyor
            if (processCreateDTO.AmountSent>_feeRequiringApproval)
            {
                processCreateDTO.ProcessStatus=ProcessStatus.WaitingForApproval;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                return AppResult<ProcessDTO>.Fail(StatusCodes.Status400BadRequest, "Bu işlem için yönetici izni gerekli.");
            }

            if (processCreateDTO.ProcessType==ProcessType.SendingMoney||processCreateDTO.ProcessType==ProcessType.Payment)
            {
                //Bakiye kontrolü yapılıyor
                if (processCreateDTO.AmountSent>_account.AccountBalance)
                {
                    //return AppResult<AccountDTO>.Fail(StatusCodes.Status400BadRequest, "Hesabınızda yeterli bakiye bulunmamaktadır.");
                    processCreateDTO.ProcessStatus=ProcessStatus.unSuccessful;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.");
                }
                _account.AccountBalance-=processCreateDTO.AmountSent;
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(_account));
                var outgoingAccountDTO = _accountRepository.GetAccountByExpAsync(x => x.AccountNumber==processCreateDTO.OutgoingAccountNumber);
                outgoingAccountDTO.Result.AccountBalance+=processCreateDTO.AmountSent;
                var resultOutgoingAccountDTO = _accountRepository.Update(outgoingAccountDTO.Result);
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
            }


            if (processCreateDTO.ProcessType==ProcessType.WithdrawMoney)
            {
                //Bakiye kontrolü yapılıyor
                if (processCreateDTO.AmountSent>_account.AccountBalance)
                {
                    //return AppResult<AccountDTO>.Fail(StatusCodes.Status400BadRequest, "Hesabınızda yeterli bakiye bulunmamaktadır.");
                    processCreateDTO.ProcessStatus=ProcessStatus.unSuccessful;
                    processCreateDTO.AppUserId=appUser.Id;
                    _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                    throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.");
                }
                _account.AccountBalance-=processCreateDTO.AmountSent;
                processCreateDTO.OutgoingAccountNumber="";
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(_account));
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
            }

            if (processCreateDTO.ProcessType==ProcessType.DepozitMoney)
            {
                _account.AccountBalance+=processCreateDTO.AmountSent;
                processCreateDTO.ProcessStatus=ProcessStatus.Successful;
                processCreateDTO.OutgoingAccountNumber=_account.AccountNumber;
                processCreateDTO.AppUserId=appUser.Id;
                _processRepository.AddAsync(_mapper.Map<Process>(processCreateDTO));
                var isUpdateSuccess = _accountRepository.Update(_mapper.Map<Account>(_account));
                var _isItTrue = _uow.saveChanges();
                return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
            }
            return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _mapper.Map<ProcessDTO>(processCreateDTO));
        }

        public async Task<IAppResult<ProcessDTO>> MakeAutomaticPayment(ProcessCreateDTO CreateDTO)
        { 
            //Gönderici ve alıcının hesaplarını çağırır.
            var accountDTO = await _accountRepository.GetAccountByExpAsync(x => x.Id==CreateDTO.AccountId);
            if (accountDTO.AccountBalance<CreateDTO.AmountSent)
            {
                throw new Exception("Hesabınızda yeterli bakiye bulunmamaktadır.Otomatik ödeme yapılamamıştır.");
            }
            var outgoingAccountDTO = await _accountRepository.GetAccountByExpAsync(x => x.AccountNumber==CreateDTO.OutgoingAccountNumber);
          
            CreateDTO.ProcessStatus =ProcessStatus.Successful;
            CreateDTO.IsActive=true;
            _processRepository.Update(_mapper.Map<Process>(CreateDTO));
            //Gönderici ve alıcının hesaplarından para transferlerini yapar.
            outgoingAccountDTO.AccountBalance+=CreateDTO.AmountSent;
            accountDTO.AccountBalance-=CreateDTO.AmountSent;
            _accountRepository.Update(_mapper.Map<Account>(outgoingAccountDTO));
            _accountRepository.Update(_mapper.Map<Account>(accountDTO));
            //Otomatik ödeme için bir dahaki aya işlem oluşturur.
            var process = _mapper.Map<Process>(CreateDTO);
            process.PaymentDate=process.PaymentDate.AddMonths(1);
            process.ProcessStatus =ProcessStatus.FuturePayment;
            process.Id=0;
            _processRepository.AddAsync(process);
            var a = _uow.saveChanges();
            var _processDTO = _mapper.Map<ProcessDTO>(process);
            return AppResult<ProcessDTO>.Success(StatusCodes.Status200OK, _processDTO);
        }
        //Otomatik ödemesi yapılan işlemi günceller
        public void UpdatesTransactionsInAutomaticPayment(ProcessCreateDTO processCreateDTO)
        {

            processCreateDTO.ProcessStatus =ProcessStatus.Successful;
            _processRepository.Update(_mapper.Map<Process>(processCreateDTO));

        }
    }
}