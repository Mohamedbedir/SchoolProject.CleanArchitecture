using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Models
{
    public class UpdateUserClaimsCommand : ManageUserClaimsResponse,IRequest<Response<string>>
    {
    }
}
