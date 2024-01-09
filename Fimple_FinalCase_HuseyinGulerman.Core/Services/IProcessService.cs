using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Services
{
    public interface IProcessService:IService<Process,ProcessCreateDTO,ProcessDTO>
    {
        Task<IAppResult<ProcessDTO>> AddSendingMoneyAndPaymentAsync(AppUser appUser, ProcessCreateDTO processCreateDTO,AccountDTO account);
        Task<IAppResult<ProcessDTO>> AddWithdrawMoneyAsync(AppUser appUser, ProcessCreateDTO processCreateDTO,AccountDTO account);
        Task<IAppResult<ProcessDTO>> AddDepositMoneyAsync(AppUser appUser, ProcessCreateDTO processCreateDTO,AccountDTO account);
        Task<IAppResult<ProcessDTO>> AddAutomaticAsync(AppUser appUser, ProcessAutomaticPaymentCreateDTO processCreateDTO);
        Task<IAppResult<ProcessDTO>> MakeAutomaticPayment(ProcessCreateDTO CreateDTO);
    
    }
}
