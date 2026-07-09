using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.Repos.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Repos
{
    public class UserRefreshTokenRepo :GenericRepos<UserRefreshToken>, IUserRefreshTokenRepo
    {
        private readonly SchoolDbContext dbContext;

        public UserRefreshTokenRepo(SchoolDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}
