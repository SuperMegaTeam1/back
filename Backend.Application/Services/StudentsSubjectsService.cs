using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.StudentsSubjects;

namespace Backend.Application.Services
{
    public class StudentsSubjectsService : IStudentsSubjectsService
    {
        public readonly IStudentRepository _studentRepo;
        public readonly ISubjectRepository _subjectRepo;

        public StudentsSubjectsService(IStudentRepository studentRepository, ISubjectRepository subjectRepository)
        {
            _studentRepo = studentRepository;
            _subjectRepo = subjectRepository;
        }

        public async Task<IReadOnlyList<StudentsSubjectsResponse>> GetSubjectsForStudentAsync(Guid userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId) ?? throw new Exception("Student not found");
            var studentId = student.Id;
            var groupId = student.StudyGroupId;
            var subjects = await _subjectRepo.GetSubjectsByStudyGroupIdAsync(groupId);
            return subjects.Select(s => new StudentsSubjectsResponse
            {
                SubjectId = s.Id,
                SubjectName = s.Name
            }).ToList();
        }
    }
}
