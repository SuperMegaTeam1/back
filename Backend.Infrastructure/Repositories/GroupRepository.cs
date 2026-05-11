using System.Text.RegularExpressions;
using Backend.Application.Interfaces;
using Backend.Application.Models.Group;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _dbContext;
    
    public GroupRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StudyGroup?> GetGroupAsync(Guid groupId)
    {
        return await _dbContext.StudyGroups.FirstOrDefaultAsync(x => x.Id == groupId);
    }

    public async Task<List<GroupsTeacherDto>> GetGroupsByTeacherAsync(Guid userId)
    {
        return await _dbContext.StudyGroups
            .Where(group => group.Lessons
                .Any(lesson => lesson.Teacher.ParentUserId == userId))
            .Distinct()
            .Select(g => new GroupsTeacherDto(
                g.Id,
                g.Name))
            .ToListAsync();
    }
}