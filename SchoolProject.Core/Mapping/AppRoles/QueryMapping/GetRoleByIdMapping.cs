using Microsoft.AspNetCore.Identity;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.AppRoles
{
    public partial class RoleProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<IdentityRole, GetRoleByIdResponse>();
        }
    }
}
