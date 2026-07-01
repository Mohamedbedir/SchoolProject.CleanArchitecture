using Microsoft.EntityFrameworkCore;
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
    public class InstructorRepo:GenericRepos<Instructor>, IInstructorRepo
    {
        private readonly SchoolDbContext dbContext;


        public InstructorRepo(SchoolDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}
