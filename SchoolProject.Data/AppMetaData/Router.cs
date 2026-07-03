using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string root = "Api";
        public const string Version = "v1";
        public const string Rule = root+"/"+Version+"/";

        public static class StudentRouting
        {
            public const string Prefix = Rule+"Student";
            public const string List = Prefix+"/List";
            public const string Paginated = Prefix+ "/Paginated";
            public const string ById = Prefix + "/ById"+ "/{id}";
            public const string Create = Prefix + "/Create";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete" + "/{id}";
        }
        public static class SubjectRouting
        {
            public const string Prefix = Rule+ "Subject";
            public const string List = Prefix+"/List";
            public const string ById = Prefix + "/ById"+ "/{id}";
            public const string Create = Prefix + "/Create";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete"+ "/{id}";
        }
        public static class DepartmentRouting
        {
            public const string Prefix = Rule+ "Department";
            public const string List = Prefix+"/List";
            public const string ById = Prefix + "/ById"+ "/{id}";
            public const string Create = Prefix + "/Create";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete"+ "/{id}";
        }
        public static class UserRouting
        {
            public const string Prefix = Rule+ "User";
            public const string List = Prefix+"/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string ById = Prefix + "/ById"+ "/{id}";
            public const string Create = Prefix + "/Create";
            public const string Update = Prefix + "/Update";
            public const string ChangePassword = Prefix + "/Change-Password";
            public const string LockUser = Prefix + "/Lock-User" + "/{id}";
            public const string UnLockUser = Prefix + "/UnLock-User" + "/{id}";
            public const string Delete = Prefix + "/Delete"+ "/{id}";
        }
    }
}
