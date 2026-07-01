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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IStudentRepo studentRepo;

        public DepartmentService(IDepartmentRepo departmentRepo ,IStudentRepo studentRepo )
        {
            this.departmentRepo = departmentRepo;
            this.studentRepo = studentRepo;
        }
        public async Task<Department> GetDepartmentById(int id)
        {
            var Department = await departmentRepo.GetTableNoTracking()
                            .Where(x => x.Id == id)
                            .Include(s=>s.DepartmentSubjects).ThenInclude(s=>s.Subject)
                            .Include(s=>s.Students)
                            .Include(i=>i.Instructors)
                            .Include(m=>m.Manager)
                            .FirstOrDefaultAsync();
            return Department;
        }

        public Task<bool> IsDepartmentIdExist(int id)
        {
            return departmentRepo.GetTableNoTracking().AnyAsync(d=>d.Id == id);
        }

        public async Task<bool> IsDepartmentIdExistExcludeSelf(int DId, int studid)
        {
            return await studentRepo.GetTableNoTracking().AnyAsync(d => d.DID == DId && d.Id==studid);
        }
    }
}
