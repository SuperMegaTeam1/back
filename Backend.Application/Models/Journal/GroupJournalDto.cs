using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Journal
{
    public class GroupJournalDto
    {
        public Guid LessonId { get; set; }
        public DateTime Date { get; set; }
        public Guid StudentId { get; set; }
        public bool? Attended { get; set; }
        public int? Grade { get; set; }
    }
}
