using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Controllers
{
    [Route("api/v1/tokens")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
           
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] TokenCreateDTO tokenCreateDTO   )
        {
            var response = await _tokenService.Login(tokenCreateDTO);
            if (response.Errors is not null)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
