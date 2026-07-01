using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities
{
    public class DepartmentSubject
    {
       
        public int DID { get; set; }
        public int SubID { get; set; }

        [ForeignKey("DID")]
        public virtual Department? Department { get; set; }

        [ForeignKey("SubID")]
        public virtual Subject? Subject { get; set; }
    }
}
