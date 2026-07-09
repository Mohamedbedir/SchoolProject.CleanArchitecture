using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Data
{
    public class SchoolDbContext:IdentityDbContext<AppUser>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.Entity<DepartmentSubject>(options =>
            //{
            //    options.HasKey(e => new { e.SubID, e.DID });

            //    options.HasOne(d => d.Department)
            //             .WithMany(ds => ds.DepartmentSubjects)
            //             .HasForeignKey(d => d.DID)
            //             .OnDelete(DeleteBehavior.SetNull);

            //    options.HasOne(s => s.Subject)
            //             .WithMany(ds => ds.DepartmentsSubjects)
            //             .HasForeignKey(d => d.SubID)
            //             .OnDelete(DeleteBehavior.SetNull);
            //});

            //modelBuilder.Entity<StudentSubject>(options =>
            //{
            //    options.HasKey(e => new { e.SubID, e.StudID });

            //    options.Property(g => g.Grade)
            //           .HasColumnType("decimal(5,2");

            //    options.HasOne(s => s.Student)
            //             .WithMany(ds => ds.StudentsSubjects)
            //             .HasForeignKey(s => s.SubID)
            //             .OnDelete(DeleteBehavior.SetNull);

            //    options.HasOne(s => s.Subject)
            //             .WithMany(ss => ss.StudentsSubjects)
            //             .HasForeignKey(d => d.SubID)
            //             .OnDelete(DeleteBehavior.SetNull);
            //});

            //modelBuilder.Entity<InstructorSubject>(options =>
            //{
            //    options.HasKey(e => new { e.SubID, e.InsID });

            //    options.HasOne(i => i.Instructor)
            //             .WithMany(Is => Is.InstructorSubjects)
            //             .HasForeignKey(i => i.InsID)
            //             .OnDelete(DeleteBehavior.SetNull);

            //    options.HasOne(s => s.Subject)
            //             .WithMany(Is => Is.InstructorSubjects)
            //             .HasForeignKey(d => d.SubID)
            //             .OnDelete(DeleteBehavior.SetNull);
            //});

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<DepartmentSubject> DepartmentSubjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<InstructorSubject> InstructorSubjects { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
