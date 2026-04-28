using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Infrastructure.Identity;
using Backend.Domain.Entities;
using System.Text.RegularExpressions;
using Backend.Domain.Interfaces;

namespace Backend.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudyGroup> StudyGroups { get; set; }
    public DbSet<SubjectEntity> Subjects { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<LessonParticipation> LessonParticipations { get; set; }
    public DbSet<StudentGrade> StudentGrades { get; set; }
    public DbSet<StudentRating> StudentRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        UpdateTimeStamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimeStamps()
    {
        var entries = ChangeTracker.Entries<IEntityWithChangeInfo>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
