using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Validators
{
    public class AddRoleValidator:AbstractValidator<AddRoleCommand>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly RoleManager<IdentityRole> roleManager;

        public AddRoleValidator(IStringLocalizer<SharedResources> localizer
            ,UserManager<AppUser> userManager
            ,RoleManager<IdentityRole> roleManager)
        {

            this.localizer = localizer;
            this.roleManager = roleManager;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(s => s.RoleName)
                .NotEmpty().WithMessage($"{localizer[SharedResourcesKeys.Empty]}");
                
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(s => s.RoleName)
               .MustAsync(async (key, cancellationToken) => !await roleManager.RoleExistsAsync(key))
               .WithMessage(localizer[SharedResourcesKeys.RoleExist]);


        }
    }
}
