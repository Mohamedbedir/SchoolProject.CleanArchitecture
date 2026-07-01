using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;

//using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Localization;
using SchoolProject.Core.Pagination;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentsQueryHandler : ResponseHandler
        ,IRequestHandler<GetStudentsQuery,Response<IReadOnlyList<GetStudentsResponse>>>
        ,IRequestHandler<GetStudentByIdQuery,Response<GetStudentByIdResponse>>
        ,IRequestHandler<GetStudentsPaginatedQuery,PaginatedResult<GetStudentsResponse>>
    {
        private readonly IStudentService studentService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public StudentsQueryHandler(IStudentService studentService
                                    ,IMapper mapper
                                    ,IStringLocalizer<SharedResources> stringLocalizer):base(stringLocalizer)
        {
            this.studentService = studentService;
            this.mapper = mapper;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<IReadOnlyList<GetStudentsResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var studentlist= await studentService.GetStudentsAsync();
            var studentlistMapper=mapper.Map<IReadOnlyList<GetStudentsResponse>>(studentlist);
            //return Success(studentlistMapper);
            return Success(studentlistMapper, Meta: new { CountData = studentlist.Count});
        }

        public async Task<Response<GetStudentByIdResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var Id= request.Id;
            var student = await studentService.GetStudentByIdAsync(Id);
            if (student == null)
                return NotFound<GetStudentByIdResponse>(stringLocalizer[SharedResourcesKeys.NotFound]);
            var studentMapper = mapper.Map<GetStudentByIdResponse>(student);
            //return Success(studentlistMapper);
            return Success(studentMapper);
        }

        public async Task<PaginatedResult<GetStudentsResponse>> Handle(GetStudentsPaginatedQuery request, CancellationToken cancellationToken)
        {
           // Expression<Func<Student, GetStudentsResponse>> expression = e => 
             //  new GetStudentsResponse(e.StudID,e.Name,e.Address,e.Department.DName);
            var Queryable = await studentService.FilterStudentPaginatedQueryable(request.OrderBy,request.Search);
            var paginetedList = await Queryable.Select(s => new GetStudentsResponse
            {
                Id = s.Id,
                Name= s.Name,
                Address= s.Address,
                Phone= s.Phone,
                DepartmentName=s.Department.Name
            }).ToPaginatedListAsync(request.PageNumber,request.PageSize);
            return paginetedList;
        }
    }
}
