using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> localizer)
        {

            this.localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Id)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            //.NotNull().WithMessage("Name Mustn't Be Null")
            //.MaximumLength(100).WithMessage(string.Format(localizer["MaxLength"], 100, localizer["Chars"]));
            RuleFor(s => s.OldPassword)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
                //.NotEmpty().WithMessage("{PropertyName} Mustn't Be Empty")
                //.NotNull().WithMessage("Name Mustn't Be Null")

           
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage(localizer[SharedResourcesKeys.uppercase])
                .Matches("[a-z]").WithMessage(localizer[SharedResourcesKeys.lowercase])
                .Matches("[0-9]").WithMessage(localizer[SharedResourcesKeys.digit])
                .Matches("[^a-zA-Z0-9]").WithMessage(localizer[SharedResourcesKeys.specialchar]);

            RuleFor(s => s.ConfirmPassword)
               .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}")
               .Equal(p => p.NewPassword).WithMessage($"{localizer[SharedResourcesKeys.PCNotEqual]}")
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
