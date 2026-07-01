using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class InstructorSubject
    {

        public int SubID { get; set; }
        public int InsID { get; set; }

        [ForeignKey("SubID")]
        public virtual Subject? Subject { get; set; }
        

        [ForeignKey("InsID")]
        public virtual Instructor? Instructor { get; set; }
    }
}