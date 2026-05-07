using Backend.Application.Models.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IJournalService
    {
        Task<JournalResponse> UpdateJournal(Guid lessonId, UpdateJournalRequest request);
    }
}
