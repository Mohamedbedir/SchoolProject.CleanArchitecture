using AutoMapper;
using SchoolProject.Core.Features.Subjects.Query.Response;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Subjects
{
    public partial class SubjectProfile 
    {
        public void GetSubjectsMapping()
        {
            CreateMap<Subject, GetSubjectsResponse>();

        }
    }
}
