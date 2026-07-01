using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class AppUsersCommandHandler : ResponseHandler,
        IRequestHandler<AddAppUserCommand,Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly UserManager<AppUser> userManager;

        public AppUsersCommandHandler(IStringLocalizer<SharedResources> localizer
            , UserManager<AppUser> userManager) :base(localizer)
        {
            this.localizer = localizer;
            this.userManager = userManager;
        }

        public async Task<Response<string>> Handle(AddAppUserCommand request, CancellationToken cancellationToken)
        {
            var user= await userManager.FindByEmailAsync(request.Email);
            if (user != null) return BadRequest<string>(localizer[SharedResourcesKeys.EmailExist]);

            var usermapped = new AppUser()
            {
                Email = request.Email,
                UserName = request.Email.Split('@')[0],
                FullName = request.FullName,
                Address=request.Address,
                Country=request.Country

            };

            var res = await userManager.CreateAsync(usermapped, request.Password);

            if (!res.Succeeded)
            {
                var errors = string.Join(Environment.NewLine,
                                         res.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }

            return Created<string>("");
        }
    }
}
