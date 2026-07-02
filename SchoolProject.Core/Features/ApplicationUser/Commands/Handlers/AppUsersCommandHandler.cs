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
        IRequestHandler<AddAppUserCommand,Response<string>>,
        IRequestHandler<EditAppUserCommand,Response<string>>,
        IRequestHandler<DeleteAppUserCommand,Response<string>>,
        IRequestHandler<ChangeUserPasswordCommand, Response<string>>
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
                Country=request.Country,
                PhoneNumber=request.Phone,

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

        public async Task<Response<string>> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null)
                return NotFound<string>();
            var res= await userManager.DeleteAsync(user);
            if (!res.Succeeded) 
            {
                return BadRequest<string>(localizer[SharedResourcesKeys.FailedDelete]);
            }
            return Deleted<string>();
        }

        public async Task<Response<string>> Handle(EditAppUserCommand request, CancellationToken cancellationToken)
        {
            var User = await userManager.FindByIdAsync(request.Id);
            if (User == null)
                return NotFound<string>();
            User.FullName = request.FullName;
            User.Address = request.Address;
            User.PhoneNumber = request.Phone;
            User.Email = request.Email;
            User.Country= request.Country;
            User.UserName=request.Email.Split('@')[0];
            var res=await userManager.UpdateAsync(User);
            if (!res.Succeeded)
            {
                var errors = string.Join(Environment.NewLine,
                                         res.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }

            return Success<string>("");

        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var User = await userManager.FindByIdAsync(request.Id);
            if (User == null)
                return NotFound<string>();
            var res= await userManager.ChangePasswordAsync(User,request.OldPassword,request.NewPassword);
            if (!res.Succeeded)
            {
                var Erorrs = string.Join(Environment.NewLine,  res.Errors.Select(e => e.Description));
                return BadRequest<string>(Erorrs);
            }
            return Success("",Message: localizer[SharedResourcesKeys.ChangePass]);
        }
    }
}
