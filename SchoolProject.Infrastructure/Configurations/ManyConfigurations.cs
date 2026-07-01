using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Configurations
{
    public class DepartmentSubjectConfigurations : IEntityTypeConfiguration<DepartmentSubject>
    {
        public void Configure(EntityTypeBuilder<DepartmentSubject> builder)
        {
            builder.HasKey(e => new { e.SubID, e.DID });

            builder.HasOne(d => d.Department)
                     .WithMany(ds => ds.DepartmentSubjects)
                     .HasForeignKey(d => d.DID)
                     /*.OnDelete(DeleteBehavior.SetNull)*/;

            builder.HasOne(s => s.Subject)
                     .WithMany(ds => ds.DepartmentsSubjects)
                     .HasForeignKey(d => d.SubID)
                     /*.OnDelete(DeleteBehavior.SetNull)*/;
        }
    }
    public class StudentSubjectConfigurations : IEntityTypeConfiguration<StudentSubject>
    {
        public void Configure(EntityTypeBuilder<StudentSubject> builder)
        {
            builder.HasKey(e => new { e.SubID, e.StudID });

            builder.Property(g => g.Grade)
                   .HasColumnType("decimal(5,2)");

            builder.HasOne(s => s.Student)
                     .WithMany(ds => ds.StudentsSubjects)
                     .HasForeignKey(s => s.SubID)
                    /* .OnDelete(DeleteBehavior.SetNull)*/;

            builder.HasOne(s => s.Subject)
                     .WithMany(ss => ss.StudentsSubjects)
                     .HasForeignKey(d => d.SubID)
                     /*.OnDelete(DeleteBehavior.SetNull)*/;
        }
    }
    public class InstructorSubjectConfigurations : IEntityTypeConfiguration<InstructorSubject>
    {
        public void Configure(EntityTypeBuilder<InstructorSubject> builder)
        {
            builder.HasKey(e => new { e.SubID, e.InsID });

            builder.HasOne(i => i.Instructor)
                     .WithMany(Is => Is.InstructorSubjects)
                     .HasForeignKey(i => i.InsID)
                     /*.OnDelete(DeleteBehavior.SetNull)*/;

            builder.HasOne(s => s.Subject)
                     .WithMany(Is => Is.InstructorSubjects)
                     .HasForeignKey(d => d.SubID)
                     /*.OnDelete(DeleteBehavior.SetNull)*/;
        }
    }
}


