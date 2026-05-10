using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.StudentsSubjects
{
    public class SubjectsResponse
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
    }
}
