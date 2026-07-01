using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.Repos.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Repos
{
    public class DepartmentRepo:GenericRepos<Department>, IDepartmentRepo
    {
        private readonly SchoolDbContext dbContext;

        public DepartmentRepo(SchoolDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}
