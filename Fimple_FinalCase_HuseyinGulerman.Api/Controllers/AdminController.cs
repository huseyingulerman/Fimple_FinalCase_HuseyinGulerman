using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/v1/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public AdminController(UserManager<AppUser> appUserService, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _appUserService = appUserService;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        [HttpPost("addrole/{email}")]
        public async Task<IActionResult> AddRole( string email, AppUserRoleCreateDTO appUserRoleCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var appUserRole = _mapper.Map<IdentityRole>(appUserRoleCreateDTO);
                var resultRole = await _roleManager.FindByNameAsync(appUserRole.Name);
                if (resultRole is null)
                {
                    return BadRequest("Rol bulunamadı.");
                }
                if (email is null)
                {
                    return BadRequest("Rol eklenecek kullanıcının email adresini giriniz.");
                }
                var resultUser = await _appUserService.FindByEmailAsync(email);
                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
                var result = await _appUserService.AddToRoleAsync(resultUser, resultRole.Name);
                //IdentityResult result = await _appUserService.CreateAsync(appUser, appUserCreateDTO.Password);

                if (result.Succeeded)
                {
                    return Created("/api/users/", result);
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
            return BadRequest(appUserRoleCreateDTO);
        }

        [HttpDelete("deleterole/{email}")]
        public async Task<IActionResult> DeleteRole(string email, string role)
        {
            if (ModelState.IsValid)
            {
                var resultRole = await _roleManager.FindByNameAsync(role);
                if (resultRole is null)
                {
                    return BadRequest("Rol bulunamadı.");
                }
                if (email is null)
                {
                    return BadRequest("Rolü güncellenecek kullanıcının email adresini giriniz.");
                }
                var resultUser = await _appUserService.FindByEmailAsync(email);
                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
                var result = await _appUserService.RemoveFromRoleAsync(resultUser,resultRole.Name);
                if (result.Succeeded)
                {
                    return NoContent();
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
            return BadRequest(role);
        }
    }
}
