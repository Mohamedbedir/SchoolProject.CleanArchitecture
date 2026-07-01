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
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .HasMaxLength(500);

            builder.Property(s => s.Phone)
                .IsRequired();

            builder.Property(s => s.Age)
                .IsRequired();

            builder.HasOne(d=>d.Department)
                .WithMany(s=>s.Students)
                .HasForeignKey(d=>d.DID)
                .OnDelete(DeleteBehavior.Restrict);
           
        }
    }
}
