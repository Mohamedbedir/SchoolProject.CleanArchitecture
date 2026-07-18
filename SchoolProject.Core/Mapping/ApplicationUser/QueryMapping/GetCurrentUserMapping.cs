using SchoolProject.Core.Features.ApplicationUser.Queries.Responses;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SchoolProject.Core.Mapping.ApplicationUser
{
    public partial class AppUserProfile
    {
        public void GetCurrentUserMapping()
        {
            CreateMap<AppUser, GetCurrentUserResponse>()
                .ForMember(d=>d.Phone,o=>o.MapFrom(s=>s.PhoneNumber));
        }
    }
}
