using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Commands.Validators
{
    public class AddStudentValidator:AbstractValidator<AddStudentCommand>
    {
        private readonly IStudentService studentService;
        private readonly IDepartmentService departmentService;
        private readonly IStringLocalizer<SharedResources> localizer;

        public AddStudentValidator(IStudentService studentService,
            IDepartmentService departmentService,
           IStringLocalizer<SharedResources> localizer)
        {
            this.studentService = studentService;
            this.departmentService = departmentService;
            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
           
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .MaximumLength(20).WithMessage(string.Format(localizer["MaxLength"], 20, localizer["Chars"]));
            RuleFor(s => s.Address)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .MaximumLength(100).WithMessage(string.Format(localizer["MaxLength"], 100, localizer["Chars"]));

            RuleFor(s => s.Phone)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .Length(11).WithMessage(string.Format(localizer["Length"], 11, localizer["Num"]))
                ;


            RuleFor(s => s.DepartmentId)
               .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(s => s.Name)
                .MustAsync(async (key, cancellationToken) => !await studentService.IsStudentExist(key))
                .WithMessage(localizer[SharedResourcesKeys.NameExist]);
            When(d => d.DepartmentId != 0, () =>
            {
                RuleFor(s => s.DepartmentId)
                .MustAsync(async (key, cancellationToken) => await departmentService.IsDepartmentIdExist(key))
                .WithMessage(localizer[SharedResourcesKeys.IsNotExist]);
            });

            
        }
    }
}
