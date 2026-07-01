using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Response;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void AddStudentmapping()
        {
            CreateMap<AddStudentCommand,Student>()
                .ForMember(d => d.DID, o => o.MapFrom(s => s.DepartmentId));
        }
    }
}

