using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Emails.Commands.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public SendEmailValidator(IStringLocalizer<SharedResources> localizer)
        {

            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Massage)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
                //.NotNull().WithMessage("Name Mustn't Be Null")
                .EmailAddress();


        }
        public void ApplyCustomValidationRules()
        {
            //When(x => x.Phone != null, () =>
            //{
            //    RuleFor(x => x.Phone).Matches(@"^01[0125][0-9]{8}$").WithMessage(localizer["Invalid"]);

            //});


        }
    }
}
