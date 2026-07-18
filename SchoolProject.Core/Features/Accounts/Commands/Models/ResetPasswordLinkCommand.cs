using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Models
{
    public class ResetPasswordLinkCommand:IRequest<Response<string>>
    {
        public string UserId {  get; set; }
        public string Code {  get; set; }
        public string Password {  get; set; }
        public string ConfirmPassword {  get; set; }
    }
}
