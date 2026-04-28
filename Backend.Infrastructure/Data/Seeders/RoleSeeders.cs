using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new[] { "Student", "Teacher", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role
                    });
                }
            }
        }
    }
}
