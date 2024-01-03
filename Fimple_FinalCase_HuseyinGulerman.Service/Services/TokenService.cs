
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Concrete;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
     
        private readonly IConfiguration _configuration;
        public TokenService(UserManager<AppUser> userManager, IConfiguration configuration) 
        {
            _userManager= userManager;
    
            _configuration = configuration;
        }

        public async Task<IAppResult<TokenDTO>> Login(TokenCreateDTO tokenCreateDTO)
        {
            if (tokenCreateDTO is null)
            {
                return AppResult<TokenDTO>.Fail(StatusCodes.Status400BadRequest, "Email ve Şifrenizini Giriniz");
            }
            if (string.IsNullOrEmpty(tokenCreateDTO.Email) || string.IsNullOrEmpty(tokenCreateDTO.Password))
            {
                return  AppResult<TokenDTO>.Fail(StatusCodes.Status400BadRequest, "Email ve Şifrenizini Giriniz"); 
            }
            tokenCreateDTO.Email = tokenCreateDTO.Email.Trim().ToLower();
            tokenCreateDTO.Password = tokenCreateDTO.Password.Trim();
            var user=await _userManager.FindByEmailAsync(tokenCreateDTO.Email);
            if (!await _userManager.CheckPasswordAsync(user, tokenCreateDTO.Password) || user is null)
            {
                return AppResult<TokenDTO>.Fail(StatusCodes.Status400BadRequest, "Email veya Şifre Hatalı");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = Token(claims);
            TokenDTO tokenDTO = new TokenDTO()
            {
                AccessToken= token,
                ExpireTime= DateTime.UtcNow.AddMinutes(30),
                Email=user.Email
            };
            return AppResult<TokenDTO>.Success(StatusCodes.Status200OK, tokenDTO);
        }
        private string Token(IEnumerable<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var tokenDesc = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse( _configuration["JWT:AccessTokenExpiration"])),
                Subject = new ClaimsIdentity(claims),
                Issuer= _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDesc);

            return tokenHandler.WriteToken(token);

        }
    }
}
