using MediatR;
using Microsoft.AspNetCore.Identity;
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
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Handlers
{
    public class AccountQueryHandler : ResponseHandler,
        IRequestHandler<GetValidatorQuery, Response<string>>
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
    }
}
