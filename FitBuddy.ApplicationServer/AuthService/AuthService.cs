using FitBuddy.Core.Entities;
using FitBuddy.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.ApplicationServer.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email ,user.Email),
                new Claim(ClaimTypes.Name ,user.DisplayName)
            };

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));

            var Token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
               issuer: _configuration["JWT:ValidIssuer"],
               expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"] ?? "0")),
               claims: Claims,
               signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)

                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
            
        }
    }
}
