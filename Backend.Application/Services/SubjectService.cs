using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Application.Models.Subject;

namespace Backend.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILessonRepository _lessonRepository;
    
    public  SubjectService(
        ISubjectRepository subjectRepository,
        ITeacherRepository teacherRepository,
        IStudentRepository studentRepository,
        ILessonRepository lessonRepository)
    {
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _studentRepository = studentRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<SubjectResponse?> GetSubjectInfoAsync(Guid userId, Guid subjectId)
    {
        var student = await _studentRepository.GetByUserIdAsync(userId);
        var subject = await _subjectRepository.GetSubjectByIdAsync(subjectId);
        
        if (subject == null)
        {
            return null;
        }
        
        var teacherInfo = await _teacherRepository.GetTeacherAsync(subject.TeacherId);
        var journalInfo = await _lessonRepository.GetListDateSubjectAndGradeBySubject(student.Id, student.StudyGroupId, subjectId);
        
        return new SubjectResponse(
            subject.Id,
            subject.Name,
            teacherInfo.teacherId,
            teacherInfo.firstName,
            teacherInfo.lastName,
            teacherInfo.fatherName,
            journalInfo);
    }
}