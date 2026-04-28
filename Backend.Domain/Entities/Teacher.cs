using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class Teacher : IEntityWithId, IEntityWithChangeInfo
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid ParentUserId { get; set; }

        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;

        public string? FatherName { get; set; } = null;
    }
}
