// Backend.Tests/Integration/Fixtures/DatabaseFixture.cs
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Backend.Tests.Integration.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16")          
        .WithDatabase("moi-ivmiit-db")      
        .WithUsername("postgres")           
        .WithPassword("Rostislav_2004")     
        .Build();

    public AppDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        DbContext = new AppDbContext(options);
        await DbContext.Database.MigrateAsync();
    }

    public async Task ResetDatabase()
    {
        DbContext.StudentGrades.RemoveRange(DbContext.StudentGrades);
        DbContext.LessonParticipations.RemoveRange(DbContext.LessonParticipations);
        DbContext.Notifications.RemoveRange(DbContext.Notifications);
        DbContext.Lessons.RemoveRange(DbContext.Lessons);
        await DbContext.SaveChangesAsync(); // сначала удаляем то что ссылается на Subjects и Teachers

        DbContext.Subjects.RemoveRange(DbContext.Subjects);
        await DbContext.SaveChangesAsync(); // потом Subjects

        DbContext.Teachers.RemoveRange(DbContext.Teachers);
        DbContext.StudyGroups.RemoveRange(DbContext.StudyGroups);
        await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _container.DisposeAsync();
    }
}