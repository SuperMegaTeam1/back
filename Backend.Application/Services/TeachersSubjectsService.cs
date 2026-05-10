using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.StudentsSubjects;

namespace Backend.Application.Services
{
    public class TeachersSubjectsService : ITeachersSubjectsService
    {
        public readonly ISubjectRepository _subjectRepo;

        public TeachersSubjectsService(ISubjectRepository subjectRepository)
        {
            _subjectRepo = subjectRepository;
        }

        public async Task<IReadOnlyList<SubjectsResponse>> GetSubjectsForTeacherAsync(Guid teacherId)
        {
            var subjects = await _subjectRepo.GetSubjectsByTeacherIdAsync(teacherId);
            return subjects.Select(s => new SubjectsResponse
            {
                SubjectId = s.Id,
                SubjectName = s.Name
            }).ToList();
        }
    }
}
