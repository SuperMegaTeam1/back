using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class SubjectEntity : IEntityWithId, IEntityWithChangeInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; } = null!;

        public Guid TeacherId { get; set; }
    }
}
