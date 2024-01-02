using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService service;
        private readonly UserManager<AppUser> u;
        public TokenController(ITokenService service,UserManager<AppUser> us)
        {
            this.service = service;
            u=us;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] TokenCreateDTO tokenCreateDTO   )
        {
            var a= await u.FindByEmailAsync(tokenCreateDTO.Email);
            var response = await service.Login(tokenCreateDTO);
            return Created("/api/tokens/" , response.Data);
        }
    }
}
