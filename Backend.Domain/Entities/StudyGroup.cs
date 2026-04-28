using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class StudyGroup : IEntityWithId, IEntityWithChangeInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    }
}
