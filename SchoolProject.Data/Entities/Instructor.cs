using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities
{
    public class Instructor
    {
        public Instructor()
        {
            InstructorSubjects = new HashSet<InstructorSubject>();
            Instructors= new HashSet<Instructor>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public int? DID { get; set; }
        public virtual Department Department { get; set; }
        public virtual Department? DepartManager { get; set; }
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public int? SupervisorID { get; set; }
        public virtual Instructor Supervisor { get; set; }


    }
}
