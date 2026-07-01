using SchoolProject.Data.Entities;
using SchoolProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services.Contract
{
    public interface IStudentService
    {
        Task<IReadOnlyList<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<string> AddStudentAsync(Student student);
        Task<string> EditStudentAsync(Student student);
        Task<string> DeleteStudentAsync(Student student);
        Task<bool> IsStudentExist(string name);
        Task<bool> IsStudentExistExcludeSelf(string name,int id);
        Task<IQueryable<Student>> FilterStudentPaginatedQueryable(StudentOrderEnum orderby,string search);
    }
}
