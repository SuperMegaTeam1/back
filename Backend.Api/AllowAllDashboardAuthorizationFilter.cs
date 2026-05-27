using Hangfire.Dashboard;

namespace Backend.Api;

/// <summary>
/// Разрешает доступ к Hangfire Dashboard без ограничений по IP.
/// Используется в Development/Docker окружении.
/// </summary>
public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
