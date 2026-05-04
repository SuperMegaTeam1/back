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
    public class JournalService : IJournalService
    {
        private readonly ILessonRepository _lessonRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IGradeRepository _gradeRepo;
        private readonly IParticipationRepository _participationRepo;

        public JournalService(
        ILessonRepository lessonRepo,
        IStudentRepository studentRepo,
        IGradeRepository gradeRepo,
        IParticipationRepository participationRepo)
        {
            _lessonRepo = lessonRepo;
            _studentRepo = studentRepo;
            _gradeRepo = gradeRepo;
            _participationRepo = participationRepo;
        }

        public async Task<JournalResponse> UpdateJournal(Guid lessonId, UpdateJournalRequest request)
        {
            var lesson = await _lessonRepo.GetByIdAsync(lessonId);

            var result = new List<JournalItemDto>();

            foreach (var item in request.Items)
            {
                if ((item.Attended == null && item.Grade == null) ||
                    (item.Attended != null && item.Grade != null))
                    throw new Exception("Заполните оценку либо поле о посещаемости");

                var student = await _studentRepo.GetByUserIdAsync(item.StudentId)
                    ?? throw new Exception($"Студент {item.StudentId} не найден");

                if (item.Grade != null)
                {
                    var grade = await _gradeRepo.GetByStudentLesson(item.StudentId, lessonId);

                    if (grade == null)
                    {
                        grade = new StudentGrade
                        {
                            StudentId = item.StudentId,
                            LessonId = lessonId,
                            Grade = item.Grade.Value
                        };

                        await _gradeRepo.AddAsync(grade);
                    }
                    else
                    {
                        grade.Grade = item.Grade.Value;
                    }

                    var participation = await _participationRepo.Get(item.StudentId, lessonId);

                    if (participation == null)
                    {
                        participation = new LessonParticipation
                        {
                            StudentId = item.StudentId,
                            LessonId = lessonId,
                            Attended = true
                        };

                        await _participationRepo.AddAsync(participation);
                    }
                    else
                    {
                        participation.Attended = true;
                    }
                }
                else
                {
                    var participation = await _participationRepo.Get(item.StudentId, lessonId);

                    if (participation == null)
                    {
                        participation = new LessonParticipation
                        {
                            StudentId = item.StudentId,
                            LessonId = lessonId,
                            Attended = item.Attended!.Value
                        };

                        await _participationRepo.AddAsync(participation);
                    }
                    else
                    {
                        participation.Attended = item.Attended!.Value;
                    }
                }

                result.Add(item);
            }

            await _lessonRepo.SaveChangesAsync();

            return new JournalResponse
            {
                LessonId = lessonId,
                Items = result
            };
        }
    }
}
