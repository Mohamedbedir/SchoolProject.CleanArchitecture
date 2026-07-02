using MediatR;
using Microsoft.AspNetCore.Http;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Models
{
    public class EditAppUserCommand:IRequest<Response<string>>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        
    }
}
