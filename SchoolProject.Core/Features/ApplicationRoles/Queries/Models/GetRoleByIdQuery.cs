using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Queries.Models
{
    public class GetRoleByIdQuery:IRequest<Response<GetRoleByIdResponse>>
    {
        public string Id { get; set; }
        public GetRoleByIdQuery(string id)
        {
            Id = id;
        }
    }
}
