using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Validators
{
    internal class SendResetPasswordValidator : AbstractValidator<SendResetPasswordCommand>
    {
   
        private readonly IStringLocalizer<SharedResources> localizer;

        public SendResetPasswordValidator(
           IStringLocalizer<SharedResources> localizer)
        {
            
            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();

        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            //.NotNull().WithMessage("Name Mustn't Be Null")
           

        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}