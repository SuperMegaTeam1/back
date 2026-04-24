using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Configurations
{
    public class StudentRatingConfig : IEntityTypeConfiguration<StudentRating>
    {
        public void Configure(EntityTypeBuilder<StudentRating> builder)
        {
            builder.HasKey(x => new { x.StudentId, x.SubjectId });

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId);

            builder.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId);
        }
    }
}
