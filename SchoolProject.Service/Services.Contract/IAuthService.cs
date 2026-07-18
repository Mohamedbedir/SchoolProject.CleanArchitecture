using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services.Contract
{
    public interface IAuthService
    {
        Task<JwtTokenResponse> GetJwtToken(AppUser user);
        Task<JwtTokenResponse> GetRefreshToken(string accesstoken,string refreshtoken);
        Task<string> ValidateToken(string accesstoken);
        Task<string> SendResetPasswordCode(string email);
        Task<string> ConfirmResetPassword(string email,string code);
        Task<string> ResetPassword(string email, string password);
        Task<string> ResetPasswordLink(string userId, string password,string token);
        Task<string> SendResetPasswordLink(string email);
        Task<AppUser> GetAppUser();
    }
}
