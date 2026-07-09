using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Repos.Contract;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings jwtSettings;
        private readonly UserManager<AppUser> userManager;
        private readonly IUserRefreshTokenRepo userRefreshTokenRepo;
        private readonly ConcurrentDictionary<string,RefreshToken> Userrefreshtoken;

        public AuthService(JwtSettings jwtSettings, 
            UserManager<AppUser> userManager,
            IUserRefreshTokenRepo userRefreshTokenRepo)
        {
            this.jwtSettings = jwtSettings;
            this.userManager = userManager;
            this.userRefreshTokenRepo = userRefreshTokenRepo;
            Userrefreshtoken = new ConcurrentDictionary<string, RefreshToken>();
        }
        
        public async Task<JwtTokenResponse> GetJwtToken(AppUser user)
        {

            var (JWToken, AccessToken) = await GenerateJwtToken(user);

            var refreshtoken = new RefreshToken()
            {
                UserName = user.UserName,
                ExpireAt = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays),
                TokenString = GenerateRefreshToken()
            };
            Userrefreshtoken.AddOrUpdate(refreshtoken.TokenString, refreshtoken, (s, t) => refreshtoken);

            var RefreshRokenSaveInDb = new UserRefreshToken()
            {
                UserId = user.Id,
                AccessToken = AccessToken,
                RefreshToken=refreshtoken.TokenString,
                IsRevoked=false,
                IsUsed=true,
                CreateAt=DateTime.UtcNow,
                ExpireAt= DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays),
                JwtId=JWToken.Id,

            };

            await userRefreshTokenRepo.AddAsync(RefreshRokenSaveInDb);

            return new JwtTokenResponse()
            {
                RefreshToken = refreshtoken,
                AccessToken = AccessToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber= new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken ReadJwtToken(string AccessToken)
        {
            if (string.IsNullOrEmpty(AccessToken))
                throw new ArgumentNullException(nameof(AccessToken));
            //var handler=new JwtSecurityTokenHandler();
            //var response=handler.ReadJwtToken(AccessToken);
            //return response;
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(AccessToken);
            return jwt;
        }
        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(AppUser user)
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
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256)
                 );
            var AccessToken = new JwtSecurityTokenHandler().WriteToken(JWToken);
          
            Console.WriteLine(AccessToken); ;
            return (JWToken, AccessToken);
        }

        public async Task<JwtTokenResponse> GetRefreshToken(string accesstoken, string refreshtoken)
        {
            var JwtToken=ReadJwtToken(accesstoken);
            if(JwtToken == null || !JwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Algorithm Is Wrong");
            }
          
            if (JwtToken.ValidTo > DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token Is Not Expired");
            }
            var userid = JwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var UserRefreshToken= await userRefreshTokenRepo.GetTableNoTracking()
                                     .FirstOrDefaultAsync(a=>a.AccessToken==accesstoken
                                     && a.RefreshToken==refreshtoken 
                                     && a.UserId==userid);

            if(UserRefreshToken == null)
                throw new SecurityTokenException("RefreshToken Is Not Found");

            if(UserRefreshToken.ExpireAt<DateTime.UtcNow) 
            {
                UserRefreshToken.IsRevoked = true;
                UserRefreshToken.IsUsed = true;
                await userRefreshTokenRepo.UpdateAsync(UserRefreshToken);
                throw new SecurityTokenException("RefreshToken Is Expired");
            }

            var user=await userManager.FindByIdAsync(userid);
            if (user == null)
                throw new SecurityTokenException("User Is Not Found");

            var (JWToken, NewAccessToken) = await GenerateJwtToken(user);
            var refreshtokenresult = new RefreshToken()
            {
                TokenString = refreshtoken,
                ExpireAt = UserRefreshToken.ExpireAt,
                UserName = JwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value
            };

            return new JwtTokenResponse()
            {
                AccessToken = NewAccessToken,
                RefreshToken = refreshtokenresult
            };

        }

        public async Task<string> ValidateToken(string accesstoken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // مهم جدًا
                    ValidateIssuerSigningKey = true,

                   
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                    ClockSkew = TimeSpan.Zero
                };

              

                var principal = handler.ValidateToken(
                    accesstoken,
                    parameters,
                    out SecurityToken validatedToken);

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
