using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Models
{
    public class ConfirmEmailQuery:IRequest<Response<string>>
    {
        public string userId { get; set; }
        public string code { get; set; }
    }
}
