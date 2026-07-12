using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using SchoolProject.Core.Localization;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Queries.Handlers
{
    public class ClaimsQueryHandler:ResponseHandler ,
        IRequestHandler<ManageUserClaimsQuery,Response<ManageUserClaimsResponse>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
       private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        //private readonly IMapper mapper;

        public ClaimsQueryHandler(IStringLocalizer<SharedResources> localizer,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager
       /* IMapper mapper*/) : base(localizer)
    {
        this.localizer = localizer;
        this.roleManager = roleManager;
            this.userManager = userManager;
            //this.mapper = mapper;
        }

        public async Task<Response<ManageUserClaimsResponse>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user=await userManager.FindByIdAsync(request.UserId);
            if (user == null) 
                return NotFound<ManageUserClaimsResponse>();
            var ClaimsResponse = new ManageUserClaimsResponse();
            ClaimsResponse.UserId= user.Id;
            var userClaimsList = new List<UserClaims>();
            var UserClaimsExist = await userManager.GetClaimsAsync(user);
            foreach (var claim in ClaimStore.claims)
            {
                var userClaims=new UserClaims();
                userClaims.Type= claim.Type;
                if (UserClaimsExist.Any(c => c.Type == claim.Type))
                    userClaims.value = true;
                else
                    userClaims.value = false;

                userClaimsList.Add(userClaims);
            }
            ClaimsResponse.claims= userClaimsList;
            return Success(ClaimsResponse);

        }
    }
}
