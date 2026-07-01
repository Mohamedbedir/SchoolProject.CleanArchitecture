using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Subjects.Command.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Subjects.Command.Validators
{
    public class AddSubjectValidator:AbstractValidator<AddSubjectCommand>
    {
        private readonly ISubjectService subjectService;
        private readonly IStringLocalizer<SharedResources> localizer;

        public AddSubjectValidator(ISubjectService subjectService,
           IStringLocalizer<SharedResources> localizer)
        {
            this.subjectService = subjectService;
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
        
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(s => s.Name)
                .MustAsync(async (key, cancellationToken) => !await subjectService.IsSubjectExist(key))
                .WithMessage(localizer[SharedResourcesKeys.NameExist]);
        }
    }
}
