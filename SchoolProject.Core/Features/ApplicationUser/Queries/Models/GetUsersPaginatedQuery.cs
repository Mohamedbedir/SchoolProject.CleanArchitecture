using MediatR;
using SchoolProject.Core.Features.ApplicationUser.Queries.Responses;
using SchoolProject.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Models
{
    public class GetUsersPaginatedQuery:IRequest<PaginatedResult<GetUsersPaginatedResponse>>
    {
        public string? FullName {  get; set; }
        public int PageNumber {  get; set; }
        public int PageSize { get; set; }

    }
}
