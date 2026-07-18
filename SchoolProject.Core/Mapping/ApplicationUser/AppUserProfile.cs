using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.ApplicationUser
{
    public partial class AppUserProfile :Profile
    {
        public AppUserProfile()
        {
            GetUserByIdMapping();
            GetCurrentUserMapping();
        }
    }
}
