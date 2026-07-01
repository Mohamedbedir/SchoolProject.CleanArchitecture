using AutoMapper;
using SchoolProject.Core.Features.Subjects.Command.Models;
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
        public void AddSubjectMapping()
        {
            CreateMap<AddSubjectCommand, Subject>();
        }
    }
}