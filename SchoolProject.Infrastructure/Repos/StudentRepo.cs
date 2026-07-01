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
    public class StudentRepo : GenericRepos<Student>,IStudentRepo
    {
        private readonly SchoolDbContext dbContext;

        public StudentRepo(SchoolDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IReadOnlyList<Student>> GetStudentsAsync()
        {
            return await dbContext.Students.Include(s => s.Department).ToListAsync();
        }
        //public override async Task<Student> GetByIdAsync(int id)
        //{
        //    return await dbContext.Students.Include(s=>s.Department).FirstOrDefaultAsync(s=>s.StudID== id);  
        //}
    }
}
