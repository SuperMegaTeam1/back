using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;

        public string FatherName { get; set; } = null;
    }
}
