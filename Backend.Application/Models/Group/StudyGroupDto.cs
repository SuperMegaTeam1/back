using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Group
{
    public class StudyGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
