using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Models
{
    public class EditRoleCommand:IRequest<Response<string>>
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
