using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Features.Accounts.Queries.Models;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Handlers
{
    public class AccountQueryHandler : ResponseHandler,
        IRequestHandler<GetValidatorQuery, Response<string>>,
        IRequestHandler<ConfirmEmailQuery, Response<string>>,
        IRequestHandler<ConfirmResetPasswordQuery, Response<string>>,
        IRequestHandler<SendResetPasswordLinkQuery, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IAuthService authService;

        public AccountQueryHandler(IStringLocalizer<SharedResources> localizer,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService) : base(localizer)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authService = authService;
        }

        public async Task<Response<string>> Handle(GetValidatorQuery request, CancellationToken cancellationToken)
        {
           var validate=await authService.ValidateToken(request.AccessToken);
           if (validate == "NotExpired") 
                return Success(validate);
            return BadRequest<string>("Expired");
        }

        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return NotFound<string>();
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.code));
            var confirmEmail = await userManager.ConfirmEmailAsync(user, decodedCode);
            if (!confirmEmail.Succeeded)
                return BadRequest<string>(string.Join(Environment.NewLine,
                    confirmEmail.Errors.Select(e => e.Description)));
            return Success<string>("", Message: "Email Confirmed Successfully");
        }

        public async Task<Response<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var Result = await authService.ConfirmResetPassword(request.Email,request.Code);
            switch (Result)
            {
                case "UserNotFound": return NotFound<string>();
                case "Failed": return BadRequest<string>();
                case "Success": return Success<string>("", Message: "Operation Done Successfully");
                default: return BadRequest<string>();
            }
        }

        public async Task<Response<string>> Handle(SendResetPasswordLinkQuery request, CancellationToken cancellationToken)
        {
            var Result = await authService.SendResetPasswordLink(request.Email);
            switch (Result)
            {
                case "UserNotFound": return NotFound<string>();
                case "Failed": return BadRequest<string>();
                case "Success": return Success<string>("", Message: "Email Sent Successfully");
                default: return BadRequest<string>();
            }
        }
    }
}
