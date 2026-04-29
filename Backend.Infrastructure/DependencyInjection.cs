using Backend.Application.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();


        return services;
    }
}
