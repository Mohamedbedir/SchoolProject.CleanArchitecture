using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationRoles.Queries.Responses
{
    public class ManageUserClaimsResponse
    {
        public string UserId { get; set; }
        public List<UserClaims> claims { get; set; }
    }
    public class UserClaims
    {
        public string Type { get; set; }
        public bool value { get; set; }
    }
}
