using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Subjects;
using Backend.Application.Models.Group;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Backend.Domain.Entities;

namespace Backend.Application.Services
{
    public class TeachersSubjectsService : ITeachersSubjectsService
    {
        public readonly ISubjectRepository _subjectRepo;

        public TeachersSubjectsService(ISubjectRepository subjectRepository)
        {
            _subjectRepo = subjectRepository;
        }

        public async Task<IReadOnlyList<TeachersSubjectsResponse>> GetSubjectsForTeacherAsync(Guid teacherId)
        {
            var data = await _subjectRepo.GetSubjectsByTeacherIdAsync(teacherId);

            var result = data
                .GroupBy(x => x.SubjectId)
                .Select(g => new TeachersSubjectsResponse
                {
                    SubjectId = g.Key,
                    SubjectName = g.First().SubjectName,

                    StudyGroups = g
                        .GroupBy(x => x.GroupId)
                        .Select(gr => new StudyGroupDto
                        {
                            Id = gr.Key,
                            Name = gr.First().GroupName
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }
    }
}
