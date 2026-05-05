using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Journal
{
    public class JournalResponse
    {
        public Guid LessonId { get; set; }
        public List<JournalItemDto> Items { get; set; } = new ();
    }
}
