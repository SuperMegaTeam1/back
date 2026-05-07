using System.Text.RegularExpressions;
using Backend.Application.Interfaces;
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
}