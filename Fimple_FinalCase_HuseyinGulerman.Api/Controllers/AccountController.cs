using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Service.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUserService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> appUserService, IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _appUserService=appUserService;
            _mapper=mapper;
        }
        [HttpPost("addaccount")]
        public async Task<IActionResult> AddAccount( UserAccountCreateDTO userAccountCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var resultUser = await _appUserService.FindByIdAsync(userId);

                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
                var _accountCreateDTO=_mapper.Map<AccountCreateDTO>(userAccountCreateDTO);
                _accountCreateDTO.AppUserId=resultUser.Id;
              var _accountDTO=  _accountService.AddAccountAsync(_accountCreateDTO, resultUser.IdentificationNumber);
                if (_accountDTO.IsCompletedSuccessfully)
                {
                    //return Created("/api/accounts/", _accountDTO.Result.Data);
                    return Created("/api/accounts/", _accountDTO.Result.Data);

                }
                else
                {  
                    return BadRequest(_accountDTO.Result.Errors);
                }
            }
            return BadRequest(userAccountCreateDTO);
        }
        [HttpPatch("{accountId}")]
        public async Task<IActionResult> UpdateAccount(int accountId, string accountName)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var resultUser = await _appUserService.FindByIdAsync(userId);

                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
               var _account=await  _accountService.GetByIdAsync(accountId);
                if (_account is null)
                {
                    return BadRequest("Hesap bulunamadı.");
                }
                _account.Data.Name=accountName;
               var _accountCreateDTO= _mapper.Map<AccountUpdateDTO>(_account.Data);
                var _accountDTO = _accountService.UpdateAsync(_accountCreateDTO);
                if (_accountDTO.Result.Errors.Count() ==0  )
                {
                    return Ok(_accountDTO.Result.Data);
                }
                else
                {

                    return BadRequest(_accountDTO.Result.Errors);
                }
            }
            return BadRequest(accountName);
        }
    }
}
