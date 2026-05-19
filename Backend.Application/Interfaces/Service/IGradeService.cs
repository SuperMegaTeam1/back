using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces
{
    public interface IGradeService
    {
        Task<StudentGrade> UpdateGrade(Guid gradeId, int grade);
    }
}
