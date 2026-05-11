using Backend.Application.Models;
using Backend.Application.Models.Group;

namespace Backend.Application.Interfaces.Service;

public interface IGroupService
{
    Task<List<GroupsTecherResponse>> GetTeacherGroupsAsync(Guid userId);
    Task<List<StudentsResponse>> GetStudentsByGroupIdAsync(Guid groupId);
}