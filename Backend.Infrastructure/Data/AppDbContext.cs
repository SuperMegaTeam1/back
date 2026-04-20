using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
