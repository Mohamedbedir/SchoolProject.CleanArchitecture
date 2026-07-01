using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Subjects.Query.Response
{
    public class GetSubjectByIdResponse
    {
        public int Id { get; set; }
        //[StringLength(500)]
        public string Name { get; set; }
        public DateTime Period { get; set; }
        public int Hours { get; set; }
    }
}
