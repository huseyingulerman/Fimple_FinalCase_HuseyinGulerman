using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> appUserService, RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            _appUserService = appUserService;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager=signInManager;
        }

        /// <summary>
        /// Admin rolüne sahip kullanıcılar seçtikleri kullanıcıya rol atayabiliyorlar.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="appUserRoleCreateDTO"></param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddRole(string userId, AppUserRoleCreateDTO appUserRoleCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var appUserRole = _mapper.Map<IdentityRole>(appUserRoleCreateDTO);
                var resultRole = await _roleManager.FindByNameAsync(appUserRole.Name);
                if (resultRole is null)
                {
                    return BadRequest("Rol bulunamadı.");
                }
                if (userId is null)
                {
                    return BadRequest("Rol eklenecek kullanıcının Idsini giriniz.");
                }
                var resultUser = await _appUserService.FindByIdAsync(userId);
                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
                var result = await _appUserService.AddToRoleAsync(resultUser, resultRole.Name);

                if (result.Succeeded)
                {
                    return Created("/api/users/", result);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(appUserRoleCreateDTO);
        }


        /// <summary>
        /// Admin rolüne sahip kullanıcılar, kullanıcının rolünü silebiliyor.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteRole(string userId, string role)
        {
            if (ModelState.IsValid)
            {
                var resultRole = await _roleManager.FindByNameAsync(role);
                if (resultRole is null)
                {
                    return BadRequest("Rol bulunamadı.");
                }
                if (userId is null)
                {
                    return BadRequest("Rolü güncellenecek kullanıcının Idsini giriniz.");
                }
                var resultUser = await _appUserService.FindByIdAsync(userId);
                if (resultUser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı.");
                }
                var result = await _appUserService.RemoveFromRoleAsync(resultUser, resultRole.Name);
                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(role);
        }

        /// <summary>
        /// Admin tüm kullanıcıların bilgilerini rolleriyle birlikte getirebiliyor.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUser()
        {
            var usersWithRoles = new List<object>();
            var users=await _appUserService.Users.AsNoTracking().ToListAsync();
            foreach (var user in users)
            {
                var roles = await _appUserService.GetRolesAsync(user);
                var userWithRoles = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                };
                usersWithRoles.Add(userWithRoles);
            }
            return Ok(usersWithRoles.ToList());
        }
    }
}
