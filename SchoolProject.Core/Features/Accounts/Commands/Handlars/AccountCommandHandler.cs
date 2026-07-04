using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Handlars
{
    public class AccountCommandHandler : ResponseHandler,
        IRequestHandler<SignInCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IAuthService authService;

        public AccountCommandHandler(IStringLocalizer<SharedResources> localizer,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService):base(localizer)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authService = authService;
        }
        public async Task<Response<string>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest<string>("Invalid Login Your Email Or Password Is Wrong");
            var res = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!res.Succeeded)
            {               
                return BadRequest<string>("Invalid Login Your Email Or Password Is Wrong");
            }
            return Success<string>(await authService.GenerateJwtToken(user), Message: "Login Successfully");
        }
    }
}
