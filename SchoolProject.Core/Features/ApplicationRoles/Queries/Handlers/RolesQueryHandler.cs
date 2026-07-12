using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Models;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using SchoolProject.Core.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Queries.Handlers
{
    public class RolesQueryHandler:ResponseHandler ,
        IRequestHandler<GetRolesListQuery,Response<IReadOnlyList<GetRolesListResponse>>> ,
        IRequestHandler<GetRoleByIdQuery,Response<GetRoleByIdResponse>>
    {
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RolesQueryHandler(IStringLocalizer<SharedResources> localizer ,
            RoleManager<IdentityRole> roleManager ,
            IMapper mapper):base(localizer)
        {
            this.localizer = localizer;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<Response<IReadOnlyList<GetRolesListResponse>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var Roles = await roleManager.Roles.ToListAsync();
            var RolesMapped= mapper.Map<IReadOnlyList<GetRolesListResponse>>(Roles);
            return Success<IReadOnlyList<GetRolesListResponse>>(RolesMapped);
        }

        public async Task<Response<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var Role= await roleManager.FindByIdAsync(request.Id);
            if(Role == null) 
                return NotFound<GetRoleByIdResponse>();
            var RoleMapped = mapper.Map<GetRoleByIdResponse>(Role);
            return Success<GetRoleByIdResponse>(RoleMapped);
        }
    }
}
