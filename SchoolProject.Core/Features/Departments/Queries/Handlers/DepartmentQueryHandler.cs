using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Departments.Queries.Models;
using SchoolProject.Core.Features.Departments.Queries.Responses;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler:ResponseHandler
                  ,IRequestHandler<GetDepartmentByIdQuery,Response<GetDepartmentByIdResponse>>
    {
        private readonly IDepartmentService departmentService;
        private readonly IMapper mapper;

        public DepartmentQueryHandler(IDepartmentService departmentService,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) :base(localizer)
        {
            this.departmentService = departmentService;
            this.mapper = mapper;
        }

        public async Task<Response<GetDepartmentByIdResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await departmentService.GetDepartmentById(request.Id);
            if (department == null) 
                return NotFound<GetDepartmentByIdResponse>();
            var mapping=mapper.Map<GetDepartmentByIdResponse>(department);
            return Success<GetDepartmentByIdResponse>(mapping);
        }
    }
}
