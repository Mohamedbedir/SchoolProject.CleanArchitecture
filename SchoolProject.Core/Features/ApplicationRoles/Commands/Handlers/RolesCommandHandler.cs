using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Handlers
{
    public class RolesCommandHandler : ResponseHandler ,
        IRequestHandler<AddRoleCommand , Response<string>>,
        IRequestHandler<EditRoleCommand , Response<string>>,
        IRequestHandler<DeleteRoleCommand , Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesCommandHandler(IStringLocalizer<SharedResources> localizer ,
            RoleManager<IdentityRole> roleManager ,
            UserManager<AppUser> userManager) :base(localizer) 
        {
            this.localizer = localizer;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var IdentityRole = new IdentityRole()
            {
                Name = request.RoleName
            };
            var res= await roleManager.CreateAsync(IdentityRole);
            if (!res.Succeeded)
            {
                var errors = string.Join(Environment.NewLine,
                                     res.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }
            return Success("");
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.Id);
            if (role == null)
                return NotFound<string>();
            var Users = await userManager.GetUsersInRoleAsync(role.Name);
            if(Users.Any())
                return BadRequest<string>("This Role Is Used");
            var res = await roleManager.DeleteAsync(role);
            if (!res.Succeeded)
            {
                var errors = string.Join(Environment.NewLine,
                                     res.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }
            return Success("");
        }

        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var role=await roleManager.FindByIdAsync(request.RoleId);
            if(role== null) return NotFound<string>();

            role.Name = request.RoleName;
           
            var res=await roleManager.UpdateAsync(role);
            if (!res.Succeeded)
            {
                var erorrs=string.Join(Environment.NewLine,res.Errors.Select(res => res.Description));
                return BadRequest<string>(erorrs);
            }
            return Success("");

        }
    }
}
