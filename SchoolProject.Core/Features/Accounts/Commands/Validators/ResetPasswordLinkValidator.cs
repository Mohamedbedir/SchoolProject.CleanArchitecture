using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Validators
{
    public class ResetPasswordLinkValidator : AbstractValidator<ResetPasswordLinkCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public ResetPasswordLinkValidator(IStringLocalizer<SharedResources> localizer)
        {

            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {

            RuleFor(s => s.UserId)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            RuleFor(s => s.Code)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");

            RuleFor(x => x.Password).NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage(localizer[SharedResourcesKeys.uppercase])
                .Matches("[a-z]").WithMessage(localizer[SharedResourcesKeys.lowercase])
                .Matches("[0-9]").WithMessage(localizer[SharedResourcesKeys.digit])
                .Matches("[^a-zA-Z0-9]").WithMessage(localizer[SharedResourcesKeys.specialchar]);

            RuleFor(s => s.ConfirmPassword)
               .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
               .Equal(p => p.Password).WithMessage($"{localizer[SharedResourcesKeys.PCNotEqual]}")
               ;


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
