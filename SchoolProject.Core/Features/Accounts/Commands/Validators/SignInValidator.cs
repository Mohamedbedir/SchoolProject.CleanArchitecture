using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Validators
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        private readonly IStudentService studentService;
        private readonly IDepartmentService departmentService;
        private readonly IStringLocalizer<SharedResources> localizer;

        public SignInValidator(IStudentService studentService,
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
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
                //.NotNull().WithMessage("Name Mustn't Be Null")
            RuleFor(s => s.Password)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
                //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
                //.NotNull().WithMessage("Name Mustn't Be Null")

        }
        public void ApplyCustomValidationRules()
        {
           
        }
    }
}