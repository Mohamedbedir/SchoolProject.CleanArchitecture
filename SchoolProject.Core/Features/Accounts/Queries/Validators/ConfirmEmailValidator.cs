using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Features.Accounts.Queries.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Validators
{
    internal class ConfirmEmailValidator : AbstractValidator<ConfirmEmailQuery>
    {
        
        private readonly IStringLocalizer<SharedResources> localizer;

        public ConfirmEmailValidator(
           IStringLocalizer<SharedResources> localizer)
        {
          
            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();

        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.userId)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            //.NotNull().WithMessage("Name Mustn't Be Null")
            RuleFor(s => s.code)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
            //.NotNull().WithMessage("Name Mustn't Be Null")

        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}