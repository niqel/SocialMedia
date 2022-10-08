using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration configuration;

        public TokenController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin userLogin)
        {
            //if it is a valid user
            if (IsValidUser(userLogin))
            {
                var token = GenerateToken();
                return Ok(new { token });
            }
            return NotFound();

        }

        private bool IsValidUser(UserLogin userLogin)
        {
            return true;
        }

        private string GenerateToken()
        {
            //header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Secretkey"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);
            //claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Gustavo Melendez"),
                new Claim(ClaimTypes.Email, "niqel504@hotmail.com"),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            //payload
            var payload = new JwtPayload
                (
                    configuration["Authentication:Issuer"],
                    configuration["Authentication:Audience"],
                    claims,
                    DateTime.Now,
                    DateTime.UtcNow.AddMinutes(3)
                );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
