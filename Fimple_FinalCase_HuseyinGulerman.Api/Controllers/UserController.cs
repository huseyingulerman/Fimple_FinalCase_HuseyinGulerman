using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles ="user")]
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUserService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> appUserService,IMapper mapper, IAccountService accountService)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _accountService = accountService;

        }
        [HttpGet]
        public IEnumerable<WeatherForecast> Pet()
        {
            return new List<WeatherForecast>();
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUserCreateDTO appUserCreateDTO)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = _mapper.Map<AppUser>(appUserCreateDTO);
                IdentityResult result = await _appUserService.CreateAsync(appUser, appUserCreateDTO.Password);

                if (result.Succeeded)
                {
                    var resultRole = await _appUserService.AddToRoleAsync(appUser, "user");
                    Account account=new Account($"{appUser.FirstName}{appUser.LastName}-Vadeli Hesabım",appUser.LastName,appUser.FirstName,appUser.Id);

                   var accountCreateDTO= _mapper.Map<AccountCreateDTO>(account);
                    await _accountService.AddAccountAsync(accountCreateDTO, appUser.IdentificationNumber);
                    return Created("/api/users/", result) ;
                }
                else
                {
                    //TempData["NotCreateUser"] = "Kullanıcı oluşturulamadı.";
                    //foreach (var item in result.Errors)
                    //{
                    //    ModelState.AddModelError("", item.Description);
                    //}
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(appUserCreateDTO);
        }

    }
}
