using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Group;

namespace Backend.Application.Models.Subjects
{
    public class TeachersSubjectsResponse
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;

        public List<StudyGroupDto> StudyGroups { get; set; } = new();
    }
}
