using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings jwtSettings;
        private readonly UserManager<AppUser> userManager;

        public AuthService(JwtSettings jwtSettings, UserManager<AppUser> userManager)
        {
            this.jwtSettings = jwtSettings;
            this.userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(AppUser user)
        {
            // Private Claims
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName),
             
            };
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

            var JWToken = new JwtSecurityToken
                (
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims:Claims,
                expires:DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials:new SigningCredentials(SecretKey,SecurityAlgorithms.HmacSha256Signature)
                 );
            var AccessToken = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return AccessToken;
        }
    }
}
