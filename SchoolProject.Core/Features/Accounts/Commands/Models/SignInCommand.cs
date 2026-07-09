using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Models
{
    public class SignInCommand:IRequest<Response<JwtTokenResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
