using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repository;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public sealed class StudyGroupRepository : IStudyGroupRepository
{
    private readonly AppDbContext _db;

    public StudyGroupRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<StudyGroup>> GetAllAsync()
    {
        return await _db.StudyGroups.ToListAsync();
    }
}

