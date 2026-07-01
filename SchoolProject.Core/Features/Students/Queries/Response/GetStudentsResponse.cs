using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Queries.Response
{
    public class GetStudentsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string? DepartmentName { get; set; }

        //public GetStudentsResponse(int studID, string name, string address, string phone, string? departmentName)
        //{
        //    StudID = studID;
        //    Name = name;
        //    Address = address;
        //    Phone = phone;
        //    DepartmentName = departmentName;
        //}
    }
}
