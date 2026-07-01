using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Data.Entities
{
    public class Subject
    {
        public Subject()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
            DepartmentsSubjects = new HashSet<DepartmentSubject>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }
        //[Key]
        public int Id { get; set; }
        //[StringLength(500)]
        public string Name { get; set; }
        public DateTime Period { get; set; }
        public int Hours { get; set; }
        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
        public virtual ICollection<DepartmentSubject> DepartmentsSubjects { get; set; }
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}