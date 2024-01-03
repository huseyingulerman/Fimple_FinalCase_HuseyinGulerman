using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles ="admin")]
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUserService;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> appUserService,IMapper mapper)
        {
            _appUserService = appUserService;
            _mapper = mapper;

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
