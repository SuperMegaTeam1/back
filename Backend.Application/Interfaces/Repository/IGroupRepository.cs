using System.Collections.Generic;
using Backend.Application.Models;
using Backend.Application.Models.Group;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces;

public interface IGroupRepository
{
    Task<StudyGroup?> GetGroupAsync(Guid groupId);
    Task<List<GroupsTeacherDto>> GetGroupsByTeacherAsync(Guid userId);
    Task<List<StudentsDto>> StudentsByGroupIdAsyncDto(Guid groupId);
}