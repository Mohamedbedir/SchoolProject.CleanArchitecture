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
    public class EditAppUserValidator : AbstractValidator<EditAppUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;

        public EditAppUserValidator(IStringLocalizer<SharedResources> localizer)
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



            //RuleFor(x => x.Phone)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty().WithMessage(localizer[SharedResourcesKeys.Empty])
            //    .Matches(@"^01[0125][0-9]{8}$").WithMessage(localizer["Invalid"]);
            RuleFor(x => x.Phone)
                .Matches(@"^01[0125][0-9]{8}$").WithMessage(localizer["Invalid"])
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));
           

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
