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
    public class SubjectRepo: GenericRepos<Subject> ,ISubjectRepo
    {
        private readonly SchoolDbContext dbContext;

        public SubjectRepo(SchoolDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
