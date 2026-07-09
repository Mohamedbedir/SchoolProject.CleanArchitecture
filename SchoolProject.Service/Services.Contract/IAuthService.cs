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
    }
}
