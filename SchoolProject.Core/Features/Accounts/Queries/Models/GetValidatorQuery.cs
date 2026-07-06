using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Accounts.Queries.Models
{
    public class GetValidatorQuery : IRequest<Response<string>>
    {
        public string AccessToken {  get; set; }
    }
}
