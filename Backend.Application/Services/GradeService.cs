using Backend.Application.Interfaces;
using Backend.Application.Models.Journal;
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
        private readonly IParticipationRepository _participationRepo;
        public GradeService(IGradeRepository gradeRepo, IParticipationRepository participationRepo)
        {
            _gradeRepo = gradeRepo;
            _participationRepo = participationRepo;
        }

        public async Task<UpdateLessonMarkResult> UpdateGrade(
       Guid gradeId,
       int? grade,
       bool? attended)
        {
            var studentGrade = await _gradeRepo.GetByIdAsync(gradeId);

            if (studentGrade == null)
                throw new Exception("Оценка не найдена");

            var participation = await _participationRepo.Get(
                studentGrade.StudentId,
                studentGrade.LessonId);

            if (participation == null)
            {
                participation = new LessonParticipation
                {
                    StudentId = studentGrade.StudentId,
                    LessonId = studentGrade.LessonId
                };

                await _participationRepo.AddAsync(participation);
            }

            // Присваиваем только если есть значение
            if (attended.HasValue)
            {
                participation.Attended = attended.Value;
            }

            if (grade.HasValue)
            {
                studentGrade.Grade = grade.Value;
                participation.Attended = true; // логично помечать посещение, если ставим оценку
            }
            else if (!grade.HasValue && studentGrade != null)
            {
                await _gradeRepo.DeleteAsync(studentGrade);
                studentGrade = null;
            }

            await _gradeRepo.SaveChangesAsync();
            await _participationRepo.SaveChangesAsync();

            return new UpdateLessonMarkResult
            {
                StudentId = studentGrade?.StudentId ?? participation.StudentId,
                LessonId = studentGrade?.LessonId ?? participation.LessonId,
                Grade = studentGrade?.Grade,
                Attended = participation.Attended
            };
        }
    }
}
