using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Subjects.Command.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Subjects.Command.Handlers
{
    public class SubjectsCommandHandler : ResponseHandler,
                  IRequestHandler<AddSubjectCommand, Response<string>>,
                  IRequestHandler<UpdateSubjectCommand, Response<string>>,
                  IRequestHandler<DeleteSubjectCommand, Response<string>>
    {
        private readonly ISubjectService subjectService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> localizer;

        public SubjectsCommandHandler(ISubjectService subjectService
            ,IMapper mapper
            ,IStringLocalizer<SharedResources> localizer):base(localizer)
        {
            this.subjectService = subjectService;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        public async Task<Response<string>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = mapper.Map<Subject>(request);
            var res= await subjectService.AddSubjectAsync(subject);
            if (res == "Success")
                return Created<string>("");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject=await subjectService.GetSubjectByIdAsync(request.Id);
            if (subject == null)
                return NotFound<string>();
            var subjectMapping = mapper.Map<Subject>(request);
            var res=await subjectService.UpdateSubjectAsync(subjectMapping);
            if (res == "Success")
                return Updated<string>("");
            return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await subjectService.GetSubjectByIdAsync(request.Id);
            if (subject == null)
                return NotFound<string>();
            var res = await subjectService.DeleteSubjectAsync(subject);
            if (res == "Success")
                return Deleted<string>();
            return BadRequest<string>();
        }
    }
}
