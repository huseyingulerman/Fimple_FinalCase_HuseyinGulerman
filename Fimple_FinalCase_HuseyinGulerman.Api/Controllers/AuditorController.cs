using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "auditor")]
    [Route("api/auditors")]
    [ApiController]
    public class AuditorController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IProcessService _processService;
        private readonly UserManager<AppUser> _appUserService;
        private readonly IMapper _mapper;

        public AuditorController(IMapper mapper,IProcessService processService,IAccountService accountService,UserManager<AppUser> appUserService)
        {
            _mapper=mapper;
            _processService =processService;
            _accountService=accountService;
            _appUserService=appUserService;
        }

        [HttpGet("getallwaitingforapproval")]
        public async Task<IActionResult> GetAllWaitingForApproval()
        {
            var _processDTO =await _processService.GetAllByExpAsync(x => x.IsActive==true, x => x.ProcessStatus==ProcessStatus.WaitingForApproval);
            return Ok(_processDTO.Data);
        }
    }
}
