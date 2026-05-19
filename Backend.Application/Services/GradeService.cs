using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepo;
        public GradeService(IGradeRepository gradeRepo)
        {
            _gradeRepo = gradeRepo;
        }

        public async Task<StudentGrade> UpdateGrade(Guid gradeId, int grade)
        {
            var studentGrade = await _gradeRepo.GetByIdAsync(gradeId);
            if (studentGrade == null)
            {
                throw new Exception("Оценка не найдена");
            }
            studentGrade.Grade = grade;
            await _gradeRepo.SaveChangesAsync();
            return studentGrade;
        }
    }
}
