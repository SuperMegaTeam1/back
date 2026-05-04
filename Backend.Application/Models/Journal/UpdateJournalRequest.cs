using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Journal
{
    public class UpdateJournalRequest
    {
        public List<JournalItemDto> Items { get; set; } = new();
    }
}
