using AutoMapper;
using Azure;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    public class StudentsCommandHandler : ResponseHandler, 
        IRequestHandler<AddStudentCommand, Bases.Response<string>> ,
        IRequestHandler<DeleteStudentCommand, Bases.Response<string>> ,
        IRequestHandler<EditStudentCommand, Bases.Response<string>>

    {
        private readonly IStudentService studentService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> localizer;

        public StudentsCommandHandler(IStudentService studentService
            ,IMapper mapper
            ,IStringLocalizer<SharedResources> localizer):base(localizer)
        {
            this.studentService = studentService;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        public async Task<Bases.Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var studentMapping = mapper.Map<Student>(request);
            var res=await studentService.AddStudentAsync(studentMapping);
            //if (res == "Exist")
            //    return UnprocessableEntity<string>("Student Name Is Exist");
            if (res == "Success")
                return Created<string>("");
            else return BadRequest<string>();
        }

        public async Task<Bases.Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // check found student by Id
            var student=await studentService.GetStudentByIdAsync(request.Id);
            if(student == null)
                return NotFound<string>();
            var studentMapping = mapper.Map<Student>(request);
            var res = await studentService.EditStudentAsync(studentMapping);
            if (res == "Success")
                return Updated<string>("");
            else return BadRequest<string>();

        }

        public async Task<Bases.Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            // check found student by Id
            var student = await studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
                return NotFound<string>();
            var res = await studentService.DeleteStudentAsync(student);
            if (res == "Success")
                return Deleted<string>();
            else return BadRequest<string>();
        }
    }
}
