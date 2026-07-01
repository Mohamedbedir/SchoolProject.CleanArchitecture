using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Repos.Contract;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepo repo;

        public SubjectService(ISubjectRepo repo)
        {
            this.repo = repo;
        }

        public async Task<string> AddSubjectAsync(Subject subject)
        {
            await repo.AddAsync(subject);
            return "Success";
        }

        public async Task<string> DeleteSubjectAsync(Subject subject)
        {
            await repo.DeleteAsync(subject);
            return "Success";
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await repo.GetTableNoTracking().FirstOrDefaultAsync(s=>s.Id== id);
        }

        public async Task<IReadOnlyList<Subject>> GetSubjectsAsync()
        {
            return await repo.GetTableNoTracking().ToListAsync();
        }

        public async Task<string> UpdateSubjectAsync(Subject subject)
        {
            await repo.UpdateAsync(subject);
            return "Success";
        }

        public async Task<bool> IsSubjectExist(string name)
        {
            var SubjectExist = repo.GetTableNoTracking()
               .Where(s => s.Name == name).SingleOrDefault();
            if (SubjectExist == null)
                return false;
            return true;
        }
        public async Task<bool> IsSubjectExistExcludeSelf(string name, int id)
        {
            var SubjectExist = await repo.GetTableNoTracking()
               .Where(s => s.Name == name & !s.Id.Equals(id)).SingleOrDefaultAsync();
            if (SubjectExist == null)
                return false;
            return true;
        }
    }
}
