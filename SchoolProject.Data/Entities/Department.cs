using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities
{
    public partial class Department
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmentSubject>();
            Instructors=new HashSet<Instructor>();  
        }
        public int Id { get; set; }
        //[StringLength(500)]
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public int? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public virtual Instructor? Manager { get; set; }
    }
}
