using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Subjects.Command.Models
{
    public class AddSubjectCommand:IRequest<Response<string>>
    {
        public string Name { get; set; }
        public DateTime Period { get; set; }
        public int Hours { get; set; }
    }
}
