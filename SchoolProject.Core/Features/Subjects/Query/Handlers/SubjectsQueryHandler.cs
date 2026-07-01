using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Core.Features.Subjects.Query.Models;
using SchoolProject.Core.Features.Subjects.Query.Response;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Services;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Subjects.Query.Handlers
{
    public class SubjectsQueryHandler : ResponseHandler,
        IRequestHandler<GetSubjectsQuery, Response<IReadOnlyList<GetSubjectsResponse>>>,
        IRequestHandler<GetSubjectByIdQuery, Response<GetSubjectByIdResponse>>
    {
        private readonly ISubjectService subjectService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> localizer;

        public SubjectsQueryHandler(ISubjectService subjectService,IMapper mapper,
            IStringLocalizer<SharedResources> localizer):base(localizer)
        {
            this.subjectService = subjectService;
            this.mapper = mapper;
            this.localizer = localizer;
        }

        public async Task<Response<IReadOnlyList<GetSubjectsResponse>>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            var Subjects = await subjectService.GetSubjectsAsync();
            var subjectsMappeing=mapper.Map<IReadOnlyList<GetSubjectsResponse>>(Subjects);
            return Success(subjectsMappeing, Meta: new { CountData = Subjects.Count});
        }

        public async Task<Response<GetSubjectByIdResponse>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await subjectService.GetSubjectByIdAsync(request.Id);
           
            if (subject == null)
                return NotFound<GetSubjectByIdResponse>();
            var subjectMapper = mapper.Map<GetSubjectByIdResponse>(subject);
            //return Success(studentlistMapper);
            return Success(subjectMapper);
        }
    }
}
