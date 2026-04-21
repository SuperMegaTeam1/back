using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        public Guid GroupId { get; set; }

        public Guid TeacherId { get; set; }

        public Guid SubjectId {  get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }
    }
}
