using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validators
{
    public class AddAppUserValidator:AbstractValidator<AddAppUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public AddAppUserValidator(IStringLocalizer<SharedResources> localizer)
        {
           
            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.FullName)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .MaximumLength(100).WithMessage(string.Format(localizer["MaxLength"], 100, localizer["Chars"]));
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .EmailAddress();

            RuleFor(s => s.Password)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .MinimumLength(8).WithMessage(string.Format(localizer["MinLength"], 8, localizer["Chars"]));

            RuleFor(s => s.ConfirmPassword)
               .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
               .Equal(p=>p.Password).WithMessage($"{localizer[SharedResourcesKeys.PCNotEqual]}");

        }
        public void ApplyCustomValidationRules()
        {
            


        }
    }
}
