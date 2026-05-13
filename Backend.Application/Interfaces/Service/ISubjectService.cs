using Backend.Application.Models.Subject;

namespace Backend.Application.Interfaces.Service;

public interface ISubjectService
{
    Task<SubjectResponse?> GetSubjectInfoAsync(Guid userId, Guid subjectId);
}