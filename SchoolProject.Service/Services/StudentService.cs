using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Enums;
using SchoolProject.Infrastructure.Repos.Contract;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _student;

        public StudentService(IStudentRepo student)
        {
            _student = student;
        }

        public async Task<string> AddStudentAsync(Student student)
        {
           
            await _student.AddAsync(student);
            return "Success";
                
        }

        public async Task<string> DeleteStudentAsync(Student student)
        {
            await _student.DeleteAsync(student);
            return "Success";
        }

        public async Task<string> EditStudentAsync(Student student)
        {
           
            await _student.UpdateAsync(student);
            return "Success";
                
        }

        public async Task<IQueryable<Student>> FilterStudentPaginatedQueryable(StudentOrderEnum orderby ,string search)
        {
            var Querable= _student.GetTableNoTracking().Include(d=>d.Department).AsQueryable();
            if (search != null)
            {
                Querable = Querable.Where(s => s.Name.Contains(search) || s.Address.Contains(search));

            }
            switch (orderby)
            {
                case StudentOrderEnum.StudID:
                    Querable = Querable.OrderBy( s => s.Id);
                    break;
                case StudentOrderEnum.Name:
                    Querable = Querable.OrderBy( s => s.Name);
                    break;
                case StudentOrderEnum.Address:
                    Querable = Querable.OrderBy( s => s.Address);
                    break;
                case StudentOrderEnum.DepartmentName:
                    Querable = Querable.OrderBy( s => s.Department.Name);
                    break;
                default:
                    Querable = Querable.OrderBy( s => s.Id);
                    break;
            }
            return Querable;
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var stud = await _student.GetTableNoTracking()
                           .Include(s => s.Department)
                           .Where(s => s.Id.Equals(id))
                           .FirstOrDefaultAsync();
            //var stud=await student.GetByIdAsync(id);
            return stud;
        }

        public async Task<IReadOnlyList<Student>> GetStudentsAsync()
        {
            return await _student.GetStudentsAsync();
        }

        public async Task<bool> IsStudentExist(string name)
        {
            var studentExist = _student.GetTableNoTracking()
               .Where(s => s.Name == name).SingleOrDefault();
            if (studentExist == null)
                return false;
            return true;
        }
        public async Task<bool> IsStudentExistExcludeSelf(string name,int id)
        {
            var studentExist =await _student.GetTableNoTracking()
               .Where(s => s.Name == name & !s.Id.Equals(id) ).SingleOrDefaultAsync();
            if (studentExist == null)
                return false;
            return true;
        }
    }
}
