using Backend.Domain.Entities;

namespace Backend.Application.Interfaces;

public interface IGroupRepository
{
    Task<StudyGroup?> GetGroupAsync(Guid groupId);
}