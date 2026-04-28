using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class StudyGroup : IEntityWithId
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

    }
}
