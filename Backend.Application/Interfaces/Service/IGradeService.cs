using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Entities;
using Backend.Application.Models.Journal;

namespace Backend.Application.Interfaces
{
    public interface IGradeService
    {
        Task<UpdateLessonMarkResult> UpdateGrade(
            Guid teacherId,
            Guid gradeId,
            int? grade,
            bool? attended);
    }
}
