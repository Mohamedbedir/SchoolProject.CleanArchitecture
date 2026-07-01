using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Repos.Contract
{
    public interface IStudentRepo:IGenericRepos<Student>
    {
        Task<IReadOnlyList<Student>> GetStudentsAsync();
    }
}
