using Backend.Application.Models;

namespace Backend.Application.Interfaces;

public interface ITeacherRepository
{
    Task<TeacherDto?> GetTeacherAsync(Guid teacherId);
}