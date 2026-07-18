using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Accounts.Queries.Models;
using SchoolProject.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Validators
{
    internal class ConfirmResetPasswordValidator : AbstractValidator<ConfirmResetPasswordQuery>
    {

        private readonly IStringLocalizer<SharedResources> localizer;

        public ConfirmResetPasswordValidator(
           IStringLocalizer<SharedResources> localizer)
        {

            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();

        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                .EmailAddress();
            //.NotNull().WithMessage("Name Mustn't Be Null")
            RuleFor(s => s.Code)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
            //.NotNull().WithMessage("Name Mustn't Be Null")

        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}