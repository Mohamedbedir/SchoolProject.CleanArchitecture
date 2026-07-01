using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services.Contract
{
    public interface IDepartmentService
    {
        Task<Department> GetDepartmentById(int id);
        Task<bool> IsDepartmentIdExist(int id);
        Task<bool> IsDepartmentIdExistExcludeSelf(int DId,int studid);
    }
}
