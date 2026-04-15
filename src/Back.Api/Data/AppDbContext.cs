using Microsoft.EntityFrameworkCore;

namespace Back.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
