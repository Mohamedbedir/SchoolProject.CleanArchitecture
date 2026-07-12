using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Commands.Handlers
{
    public class ClaimsCommandHandler : ResponseHandler,
        IRequestHandler<UpdateUserClaimsCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        //private readonly IMapper mapper;

        public ClaimsCommandHandler(IStringLocalizer<SharedResources> localizer,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager
       /* IMapper mapper*/) : base(localizer)
        {
            this.localizer = localizer;
            this.roleManager = roleManager;
            this.userManager = userManager;
            //this.mapper = mapper;
        }

        public async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            var user=await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound<string>();
            var oldclaims = await userManager.GetClaimsAsync(user);
            var RemoveOldClaimsRes=await userManager.RemoveClaimsAsync(user, oldclaims);
            if (!RemoveOldClaimsRes.Succeeded)
                return BadRequest<string>("Failed To Remove Old Claims");
            var newClaims = request.claims.Where(c => c.value == true).Select(n => new Claim(n.Type, n.value.ToString()));
            var AddNewClaimsRes = await userManager.AddClaimsAsync(user, newClaims);
            if (!AddNewClaimsRes.Succeeded)
                return BadRequest<string>("Failed To Adding New Claims");
            return Success<string>("");

        }
    }
    
}
