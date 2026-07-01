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
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .HasMaxLength(500);

            builder.Property(s => s.Position)
                .IsRequired().HasMaxLength(100);

            builder.Property(s => s.Age)
                .IsRequired();

            builder.Property(s => s.Salary)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.Department)
                .WithMany(s => s.Instructors)
                .HasForeignKey(d => d.DID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Supervisor)
                .WithMany(s => s.Instructors)
                .HasForeignKey(d => d.SupervisorID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
