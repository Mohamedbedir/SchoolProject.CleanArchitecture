using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Validators
{
    public class EditRoleValidator : AbstractValidator<EditRoleCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly RoleManager<IdentityRole> roleManager;

        public EditRoleValidator(IStringLocalizer<SharedResources> localizer
            , UserManager<AppUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {

            this.localizer = localizer;
            this.roleManager = roleManager;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.RoleId)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
            RuleFor(s => s.RoleName)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");

        }
        public void ApplyCustomValidationRules()
        {
            When(x => !string.IsNullOrWhiteSpace(x.RoleName), () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (model, cancellationToken) =>
                    {
                        var role = await roleManager.FindByNameAsync(model.RoleName);

                        if (role == null)
                            return true;

                        return role.Id == model.RoleId;
                    })
                    .WithMessage(localizer[SharedResourcesKeys.RoleExist]);
            });


        }
    }
}
