using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Data;
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
        private readonly SchoolDbContext context;
        private readonly IEmailService emailService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ConcurrentDictionary<string,RefreshToken> Userrefreshtoken;

        public AuthService(JwtSettings jwtSettings, 
            UserManager<AppUser> userManager,
            IUserRefreshTokenRepo userRefreshTokenRepo ,
            SchoolDbContext context ,
            IEmailService emailService , 
            IHttpContextAccessor httpContextAccessor)
        {
            this.jwtSettings = jwtSettings;
            this.userManager = userManager;
            this.userRefreshTokenRepo = userRefreshTokenRepo;
            this.context = context;
            this.emailService = emailService;
            this.httpContextAccessor = httpContextAccessor;
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
            var UserClaims = await userManager.GetClaimsAsync(user);
            Claims.AddRange(UserClaims);

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

        public async Task<string> SendResetPasswordCode(string email)
        {
            var trans = await context.Database.BeginTransactionAsync();
            try
            {
                var user=await userManager.FindByEmailAsync(email);
                if (user == null)
                    return "UserNotFound";
                var generator = new Random();
                var rondomNumber = generator.Next(0, 1000000).ToString("D6");
                user.Code = rondomNumber;
                var res=await userManager.UpdateAsync(user);
                if (!res.Succeeded)
                    return "ErrorInUpdateUser";
                var message = $"This Code To Reset Password : {user.Code}";
                var SendEmail = await emailService.SendEmailAsync(email, message, "Reset Password");
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                var error = ex.InnerException?.Message;
                return "Failed";
            }
        }

        public async Task<string> ConfirmResetPassword(string email, string code)
        {
            var user=await userManager.FindByEmailAsync(email);
            if (user == null)
                return "UserNotFound";
            if (user.Code == code)
                return "Success";
            return "Failed";
        }
        public async Task<string> ResetPassword(string email, string password)
        {
            var trans = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return "UserNotFound";
                await userManager.RemovePasswordAsync(user);
                await userManager.AddPasswordAsync(user, password);
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                var error = ex.InnerException?.Message;
                return "Failed";
            }
        }

        public async Task<string> ResetPasswordLink(string userId, string password, string token)
        {
            // البحث عن المستخدم
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return "UserNotFound";
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // التحقق من التوكن وتغيير كلمة المرور
            var result = await userManager.ResetPasswordAsync(user, decodedCode, password);

            if (!result.Succeeded)
            {
                return string.Join(Environment.NewLine,
                    result.Errors.Select(e => e.Description));
            }

            return "Success";
        }

        public async Task<string> SendResetPasswordLink(string email)
        {
            var trans = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return "UserNotFound";
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var ContextAccessor = httpContextAccessor.HttpContext.Request;
                var ReturnedUrl = ContextAccessor.Scheme + "://" + ContextAccessor.Host
                    + $"/Api/v1/Account/ResetPasswordLink?userId={user.Id}&code={code}";
                var message = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
    body {{
        margin:0;
        padding:0;
        background:#f4f6f9;
        font-family:Arial,Helvetica,sans-serif;
    }}

    .container {{
        max-width:600px;
        margin:40px auto;
        background:#ffffff;
        border-radius:12px;
        overflow:hidden;
        box-shadow:0 10px 30px rgba(0,0,0,.08);
    }}

    .header {{
        background:#212529;
        color:#fff;
        text-align:center;
        padding:30px;
    }}

    .header h1 {{
        margin:0;
        font-size:28px;
    }}

    .content {{
        padding:40px;
        color:#555;
        line-height:1.8;
        font-size:16px;
    }}

    .btn {{
        display:inline-block;
        margin:30px 0;
        background:#0d6efd;
        color:#fff !important;
        text-decoration:none;
        padding:15px 35px;
        border-radius:8px;
        font-weight:bold;
    }}

    .footer {{
        background:#f8f9fa;
        text-align:center;
        padding:20px;
        color:#888;
        font-size:13px;
    }}

    .warning {{
        color:#dc3545;
        font-size:14px;
        margin-top:25px;
    }}
</style>
</head>

<body>

<div class='container'>

    <div class='header'>
        <h1>School System</h1>
    </div>

    <div class='content'>

        <h2>Hello {user.FullName},</h2>

        <p>
            We received a request to reset your password.
        </p>

        <p>
            Click the button below to create a new password.
        </p>

        <div style='text-align:center;'>

            <a href='{ReturnedUrl}' class='btn'>
                Reset Password
            </a>

        </div>

        <p>
            If the button doesn't work, copy and paste this link into your browser:
        </p>

        <p style='word-break:break-all;color:#0d6efd;'>
            {ReturnedUrl}
        </p>

        <p class='warning'>
            This link will expire soon. If you didn't request a password reset,
            you can safely ignore this email.
        </p>

    </div>

    <div class='footer'>

        © 2026 School System<br/>
        This is an automated email, please do not reply.

    </div>

</div>

</body>
</html>";
                
                var SendEmail = await emailService.SendEmailAsync(email, message, "Reset Password Link");
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<AppUser> GetAppUser()
        {
            //var UserId = httpContextAccessor.HttpContext.User.Claims
            //    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userId = httpContextAccessor.HttpContext?.User?
                  .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized");
            var user =await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("Unauthorized");
            return user;
        }
    }
}
