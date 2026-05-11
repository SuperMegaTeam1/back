using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repository;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public sealed class StudentRatingRepository : IStudentRatingRepository
{
    private readonly AppDbContext _context;

    public StudentRatingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentRating>> GetByGroupAsync(Guid groupId)
    {
        return await _context.StudentRatings
            .Include(x => x.Student)
            .Where(x => x.GroupId == groupId && x.SubjectId == null)
            .OrderBy(x => x.RatingPosition)
            .ToListAsync();
    }

    public async Task<List<StudentRating>> GetByGroupAndSubjectAsync(Guid groupId, Guid subjectId)
    {
        return await _context.StudentRatings
            .Include(x => x.Student)
            .Where(x => x.GroupId == groupId && x.SubjectId == subjectId)
            .OrderBy(x => x.RatingPosition)
            .ToListAsync();
    }

    public async Task ClearAsync()
    {
        _context.StudentRatings.RemoveRange(_context.StudentRatings);

        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<StudentRating> ratings)
    {
        await _context.StudentRatings.AddRangeAsync(ratings);

        await _context.SaveChangesAsync();
    }
}