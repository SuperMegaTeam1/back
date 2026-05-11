using System.Text.RegularExpressions;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Application.Models.Group;

namespace Backend.Application.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupsRepository;
    
    public GroupService(IGroupRepository groupRepository)
    {
        _groupsRepository = groupRepository;
    }

    public async Task<List<GroupsTecherResponse>> GetTeacherGroupsAsync(Guid userId)
    {
        var teacherGroups = await _groupsRepository.GetGroupsByTeacherAsync(userId);

        if (teacherGroups == null)
        {
            return null;
        }

        return teacherGroups
            .Select(g => new GroupsTecherResponse(
                g.Id.ToString(),
                g.Name))
            .ToList();
    }
}