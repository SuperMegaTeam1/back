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
        private readonly INotificationRepository _notificationRepo;
        private readonly INotificationSender _notificationSender;
        private readonly IStudentRepository _studentRepo;

        public GradeService(
            IGradeRepository gradeRepo,
            IParticipationRepository participationRepo,
            INotificationRepository notificationRepo,
            INotificationSender notificationSender,
            IStudentRepository studentRepo
            )
        {
            _gradeRepo = gradeRepo;
            _participationRepo = participationRepo;
            _notificationRepo = notificationRepo;
            _notificationSender = notificationSender;
            _studentRepo = studentRepo;
        }

        public async Task<UpdateLessonMarkResult> UpdateGrade(
            Guid teacherId,
            Guid gradeId,
            int? grade,
            bool? attended)
        {
            var studentGrade = await _gradeRepo.GetByIdAsync(gradeId);

            if (studentGrade == null)
                throw new Exception("Оценка не найдена");

            var student = await _studentRepo.GetByIdAsync(studentGrade.StudentId)
                ?? throw new Exception("Студент не найден");

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

            if (grade.HasValue)
            {
                var notification = new Notification
                {
                    SenderId = teacherId,
                    ReceiverId = student.ParentUserId,
                    Title = $"{studentGrade.Lesson.Subject.Name}",
                    Body = $"Вам поставили {grade} за пару {studentGrade.Lesson.StartsAt:dd.MM.yyyy}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _notificationRepo.CreateNotificationAsync(notification);
                await _notificationSender.SendNotificationAsync(notification);
            }


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
