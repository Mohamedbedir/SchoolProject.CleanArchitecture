using MediatR;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Pagination;
using SchoolProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Queries.Models
{
    public class GetStudentsPaginatedQuery:IRequest<PaginatedResult<GetStudentsResponse>>
    {
        public int PageSize {  get; set; }
        public int PageNumber {  get; set; }
        public StudentOrderEnum OrderBy {  get; set; }
        public string? Search {  get; set; }
    }
}
