using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Api.Controllers;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Identity;
using Quartz;
using System.Text.RegularExpressions;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Jobs
{
    public class MakeAutomaticPaymentBackgroundJob : IJob
    {
        private readonly IProcessService _processService;
   
        private readonly IMapper _mapper;
   
        public MakeAutomaticPaymentBackgroundJob( IProcessService processService, IMapper mapper)
        {
           
            _processService = processService;
            _mapper= mapper;
        }
        public  Task Execute(IJobExecutionContext context)
        {
 
            var _processAutomaticPaymenList = _processService.GetAllByExpAsync(x => x.IsActive==true, x => x.PaymentDate<=DateTime.UtcNow, x => x.ProcessStatus==ProcessStatus.FuturePayment, x => x.ProcessType==ProcessType.AutomaticPayment);
            foreach (var _processDTO in _processAutomaticPaymenList.Result.Data)
            {
                var _processCreateDTO = _mapper.Map<ProcessCreateDTO>(_processDTO);
                var processDTO = _processService.MakeAutomaticPayment(_processCreateDTO);
            }
            return Task.FromResult(true);
        }
    }
}
