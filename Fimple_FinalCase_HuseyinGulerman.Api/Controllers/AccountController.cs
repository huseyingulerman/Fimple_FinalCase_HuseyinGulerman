using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "user,auditor")]
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
        [HttpPost("{userid}")]
        public async Task<IActionResult> AddAccount(string userid, UserAccountCreateDTO userAccountCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var resultUser = await _appUserService.FindByIdAsync(userid);
               
                if (userid is null)
                {
                    return BadRequest("Hesap açılacak kullanıcının id adresini giriniz.");
                }
              
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAccount(string id,int accountId, string accountName)
        {
            if (ModelState.IsValid)
            {
                var resultUser = await _appUserService.FindByIdAsync(id);


                if (id is null)
                {
                    return BadRequest("Hesap ismi güncellenecek kullanıcının id adresini giriniz.");
                }

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
