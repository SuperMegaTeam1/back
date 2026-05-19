using Backend.Application.Interfaces;
using Backend.Application.Models;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;
    
    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherDto?> GetTeacherAsync(Guid teacherId)
    {
        return await _context.Teachers
            .Where(teacher =>  teacher.Id == teacherId)
            .Select(teacher => new TeacherDto(
                teacher.Id,
                teacher.ParentUserId,
                teacher.FirstName,
                teacher.LastName,
                teacher.FatherName))
            .FirstOrDefaultAsync();
    }
}