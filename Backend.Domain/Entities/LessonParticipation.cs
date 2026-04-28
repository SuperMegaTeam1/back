using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class LessonParticipation
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public bool Attended { get; set; }
    }
}
