using Backend.Domain.Entities;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            AppDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            await RoleSeeder.SeedRoles(roleManager);
            await UserSeeder.SeedUsers(userManager, roleManager);
            await DomainSeeder.SeedDomainData(db, userManager);

            await db.SaveChangesAsync();
        }
    }
}
