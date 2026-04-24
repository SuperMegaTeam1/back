using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class StudentRating
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid SubjectId { get; set; }
        public SubjectEntity Subject { get; set; } = null!;

        public double Rating { get; set; }
    }
}
