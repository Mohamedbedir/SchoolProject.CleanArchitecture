using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Departments.Queries.Responses
{
    public class GetDepartmentByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }

        public List<StudentResponse> Students { get; set; }
        public List<SubjectResponse> Subjects { get; set; }
        public List<InstructorResponse> Instructors { get; set; }
       
    }
    public class StudentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
    }
    public class InstructorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
