using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Data.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUsers(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            var teacherEmail = "teacher@test.com";
            var teacher = await userManager.FindByEmailAsync(teacherEmail);

            if (teacher == null)
            {
                teacher = new ApplicationUser
                {
                    UserName = teacherEmail,
                    Email = teacherEmail,
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    FatherName= "Ivanovich"
                };

                await userManager.CreateAsync(teacher, "Password123!");
                await userManager.AddToRoleAsync(teacher, "Teacher");
            }

            var studentEmail = "student@test.com";
            var student = await userManager.FindByEmailAsync(studentEmail);

            if (student == null)
            {
                student = new ApplicationUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    FirstName = "Petr",
                    LastName = "Petrov"
                };

                await userManager.CreateAsync(student, "Password123!");
                await userManager.AddToRoleAsync(student, "Student");
            }
        }
    }
}
