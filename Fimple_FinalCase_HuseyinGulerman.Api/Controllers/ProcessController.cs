using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Service.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Transactions;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {

        private readonly IProcessService _processService;
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _appUserService;
        private readonly IMapper _mapper;
        public ProcessController(IProcessService processService, IMapper mapper, UserManager<AppUser> appUserService, IAccountService accountService)
        {
            _processService= processService;
            _mapper= mapper;
            _appUserService= appUserService;
            _accountService= accountService;
        }
        [HttpPost]
        public async Task<IActionResult> AddProcess(ProcessCreateDTO processCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var resultUser = await _appUserService.FindByIdAsync(userId);

                var account = await _accountService.GetByIdAsync(processCreateDTO.AccountId);

                var processDTO = _processService.AddProcessAsync(resultUser, processCreateDTO, account.Data);
                if (!processDTO.IsCompletedSuccessfully)
                {
                    throw new Exception("İşlem gerçekleşirken hata oluştu. Lütfen tekrar deneyiniz.");
                }
                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }

                if (processDTO.IsCompletedSuccessfully)
                {
                    return Created("/api/accounts/", processDTO.Result.Data);
                }
                else
                {

                    return BadRequest(processDTO.Result.Errors);
                }

            }
            return BadRequest(processCreateDTO);
        }

     
        [HttpPost("AddAutomaticPayment")]
        public async Task<IActionResult> AddAutomaticPayment(ProcessAutomaticPaymentCreateDTO processAutomaticPaymentCreateDTO)
        {
            if (ModelState.IsValid)
            {

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var resultUser = await _appUserService.FindByIdAsync(userId);

                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }

                var processDTO = _processService.AddAutomaticAsync(resultUser, processAutomaticPaymentCreateDTO);
                if (!processDTO.IsCompletedSuccessfully)
                {
                    throw new Exception("İşlem gerçekleşirken hata oluştu. Lütfen tekrar deneyiniz.");
                }

                if (processDTO.IsCompletedSuccessfully)
                {
                    return Created("/api/accounts/", processDTO.Result.Data);
                }
                else
                {

                    return BadRequest(processDTO.Result.Errors);
                }

            }
            return BadRequest(processAutomaticPaymentCreateDTO);
        }

        [AllowAnonymous]
        [HttpPost("MakeAutomaticPayment")]
        public async Task<IActionResult> MakeAutomaticPayment()
        {
            if (ModelState.IsValid)
            {
                var _processAutomaticPaymenList = await _processService.GetAllByExpAsync(x => x.IsActive==true, x => x.PaymentDate<=DateTime.UtcNow, x => x.ProcessStatus==ProcessStatus.FuturePayment, x => x.ProcessType==ProcessType.AutomaticPayment);
                foreach (var _processDTO in _processAutomaticPaymenList.Data)
                {
                    var _processCreateDTO = _mapper.Map<ProcessCreateDTO>(_processDTO);
                    var processDTO = await _processService.MakeAutomaticPayment(_processCreateDTO);
                }

                return Ok("/api/accounts/");
            }

            return BadRequest("İşlem başarısız.Validasyon sağlanamadı.");
        }
        [NonAction]
        public void SchedulePaymentReminderJob()
        {
            RecurringJob.AddOrUpdate(() => MakeAutomaticPayment(), Cron.Daily(3, 32));
        }
       
    }
}

