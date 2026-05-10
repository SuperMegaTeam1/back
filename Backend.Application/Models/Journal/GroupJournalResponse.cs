using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Journal
{
    public class GroupJournalResponse
    {
        public Guid SubjectId { get; set; }
        public Guid GroupId { get; set; }
        public List<GroupJournalDto> Items { get; set; } = new();
    }
}
