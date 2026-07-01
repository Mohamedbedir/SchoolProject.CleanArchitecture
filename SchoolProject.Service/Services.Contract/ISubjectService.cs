using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services.Contract
{
    public interface ISubjectService
    {
        Task<IReadOnlyList<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
        Task<string> AddSubjectAsync(Subject subject);
        Task<string> UpdateSubjectAsync(Subject subject);
        Task<string> DeleteSubjectAsync(Subject subject);

        Task<bool> IsSubjectExist(string name);

        Task<bool> IsSubjectExistExcludeSelf(string name, int id);
    }
}
