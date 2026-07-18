using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Accounts.Commands.Models;
using SchoolProject.Core.Features.Accounts.Commands.Validators;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Commands.Handlars
{
    public class AccountCommandHandler : ResponseHandler,
        IRequestHandler<SignInCommand, Response<JwtTokenResponse>>,
        IRequestHandler<RefreshTokenCommand, Response<JwtTokenResponse>>,
        IRequestHandler<SendResetPasswordCommand, Response<string>>,
        IRequestHandler<ResetPasswordCommand, Response<string>>,
        IRequestHandler<ResetPasswordLinkCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IAuthService authService;

        public AccountCommandHandler(IStringLocalizer<SharedResources> localizer,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService
            ):base(localizer)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authService = authService;
        }
        public async Task<Response<JwtTokenResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest<JwtTokenResponse>("Invalid Login Your Email Or Password Is Wrong");
            var resConfirmEmail = await userManager.IsEmailConfirmedAsync(user);
            if (!resConfirmEmail)
            {
                return BadRequest<JwtTokenResponse>("Email Not Confirmed");
            }
            var res = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!res.Succeeded)
            {               
                return BadRequest<JwtTokenResponse>("Invalid Login Your Email Or Password Is Wrong");
            }
            
            return Success<JwtTokenResponse>(await authService.GetJwtToken(user), Message: "Login Successfully");
        }

        public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var Result = await authService.SendResetPasswordCode(request.Email);
            switch (Result)
            {
                case "UserNotFound": return NotFound<string>();
                case "ErrorInUpdateUser": return BadRequest<string>("Error In Update User");
                case "Failed": return BadRequest<string>();
                case "Success": return Success<string>("",Message:"Email Sent");
                default : return BadRequest<string>();
            }
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var Result = await authService.ResetPassword(request.Email,request.Password);
            switch (Result)
            {
                case "UserNotFound": return NotFound<string>();
                case "Failed": return BadRequest<string>();
                case "Success": return Success<string>("", Message: "Reset Password Done Successfully");
                default: return BadRequest<string>();
            }
        }

        public async Task<Response<string>> Handle(ResetPasswordLinkCommand request, CancellationToken cancellationToken)
        {
            var Result = await authService.ResetPasswordLink(request.UserId, request.Password,request.Code);
            switch (Result)
            {
                case "UserNotFound": return NotFound<string>();
                case "Success": return Success<string>("", Message: "Password has been reset successfully.");
                default: return BadRequest<string>(Result);
            }
        }

        async Task<Response<JwtTokenResponse>> IRequestHandler<RefreshTokenCommand, Response<JwtTokenResponse>>.Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var res = await authService.GetRefreshToken(request.AccessToken, request.RefreshToken);
            return Success(res);
        }
    }
}
