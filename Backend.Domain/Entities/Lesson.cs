using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class Lesson : IEntityWithId, IEntityWithChangeInfo
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public Guid SubjectId { get; set; }
        public SubjectEntity Subject { get; set; } = null!;

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public ICollection<LessonParticipation> Participations { get; set; } = new List<LessonParticipation>();
        public ICollection<StudentGrade> Grades { get; set; } = new List<StudentGrade>();
    }
}
