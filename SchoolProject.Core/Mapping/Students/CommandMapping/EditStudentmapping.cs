using SchoolProject.Core.Features.Students.Commands.Models;
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
        public void EditStudentmapping()
        {
            CreateMap<EditStudentCommand, Student>()
                //.ForMember(d => d.StudID, o => o.MapFrom(s => s.StudID))
                .ForMember(d => d.DID, o => o.MapFrom(s => s.DepartmentId));
        }
    }
}
