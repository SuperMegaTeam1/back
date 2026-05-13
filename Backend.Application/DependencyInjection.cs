using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Service;
using Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IJournalService, JournalService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISubjectService, SubjectService>();

        return services;
    }
}
