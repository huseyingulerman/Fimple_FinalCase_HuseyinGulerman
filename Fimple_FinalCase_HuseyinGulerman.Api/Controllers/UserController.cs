using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUserService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> appUserService, IMapper mapper, IAccountService accountService)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _accountService = accountService;

        }


        /// <summary>
        /// Kullanıcı kaydını yapıyor. Kullanıcı oluşurken user rolü otomatik olarak veriliyor. Kullanıcı oluşurken vadeli hesabı otomatik olarak oluşuyor.
        /// </summary>
        /// <param name="appUserCreateDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUserCreateDTO appUserCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var user = _appUserService.Users.FirstOrDefault(x => x.IdentificationNumber==appUserCreateDTO.IdentificationNumber);
                if (user is not null)
                {
                    throw new Exception("Bu kimlik numaralı kullanıcının hesabı var.");
                }
                AppUser appUser = _mapper.Map<AppUser>(appUserCreateDTO);
                IdentityResult result = await _appUserService.CreateAsync(appUser, appUserCreateDTO.Password);
                var _appUser = _appUserService.Users.FirstOrDefault(x => x.IdentificationNumber==appUser.IdentificationNumber);
                if (result.Succeeded)
                {
                    var resultRole = await _appUserService.AddToRoleAsync(appUser, "user");

                    var _account = await _accountService.AddDepositAccountAsync(appUser);
                    return Created("/api/users/", _account.Data);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(appUserCreateDTO);
        }

        /// <summary>
        /// Userın sahip olduğu hesapları getiriyor.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getaccounts")]
        public async Task<IActionResult> GetAllAccount()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resultUser = await _appUserService.FindByIdAsync(userId);
            if (resultUser is null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }
            var _accountDTO =await _accountService.GetAllByExpAsync(x => x.AppUserId == userId);
            return Ok(_accountDTO.Data);

        }


        /// <summary>
        /// Accountid ye göre hesabı getiriyor.
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
        [HttpGet("getaccount/{accountid}")]
        public async Task<IActionResult> GetAccountByAccountNumber(int accountid)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resultUser = await _appUserService.FindByIdAsync(userId);
            if (resultUser is null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }
            var _accountDTO = await _accountService.GetByIdAsync(accountid);
            if (_accountDTO.Data.AppUserId==resultUser.Id)
            {
                return Ok(_accountDTO.Data);
            }
            else
            {
                return BadRequest("Kullanıcıya ait bu id de hesap yoktur.");
            }
        }

    }
}
