using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class SubjectEntity : BaseEntity
    {
        public string Name { get; set; } = null!;

        public Guid TeacherId { get; set; }
    }
}
