using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Student
    {
        public Student()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
        }
        //[Key]
        public int Id { get; set; }
        //[StringLength(200)]
        public string Name { get; set; }
        //[StringLength(500)]
        public string Address { get; set; }
        //[StringLength(500)]
        public string Phone { get; set; }
        public int Age { get; set; }
        public int? DID { get; set; }

        //[ForeignKey("DID")]
        public virtual Department Department { get; set; }
        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }

    }
}