using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Departments.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Departments.Queries.Models
{
    public class GetDepartmentByIdQuery:IRequest<Response<GetDepartmentByIdResponse>>
    {
        public int Id { get; set; }
        public GetDepartmentByIdQuery(int id) { Id = id; }
    }
}
