using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Data.Configurations
{
    public class LessonParticipationConfig : IEntityTypeConfiguration<LessonParticipation>
    {
        public void Configure(EntityTypeBuilder<LessonParticipation> builder)
        {
            builder.HasKey(x => new { x.StudentId, x.LessonId });

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId);

            builder.HasOne(x => x.Lesson)
                .WithMany(l => l.Participations)
                .HasForeignKey(x => x.LessonId);
        }
    }
}
