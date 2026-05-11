using Backend.Application.Models.Group;

namespace Backend.Application.Interfaces.Service;

public interface IGroupService
{
    Task<List<GroupsTecherResponse>> GetTeacherGroupsAsync(Guid userId);
}