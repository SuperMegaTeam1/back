using Backend.Application.Interfaces;
using Backend.Application.Models;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repository
{
    public sealed class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<AuthUser?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return null;
            }

            return await BuildAuthUserAsync(user);
        }

        public async Task<AuthUser?> FindByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return null;
            }

            return await BuildAuthUserAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<AuthUser> BuildAuthUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? string.Empty;

            Guid? studentId = null;
            Guid? teacherId = null;
            Guid? groupId = null;
            string? groupName = null;

            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.ParentUserId == user.Id);

            if (student?.StudyGroupId is not null)
            {
                studentId = student.Id;
                groupId = student.StudyGroupId;

                var group = await _dbContext.StudyGroups.FirstOrDefaultAsync(x => x.Id == student.StudyGroupId);
                groupName = group?.Name;
            }

            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(y => y.Id == user.Id);

            if (teacher is not null)
            {
                teacherId = teacher.Id;
            }

            return new AuthUser(
                Id: user.Id,
                RoleName: roleName,
                FirstName: user.FirstName ?? string.Empty,
                LastName: user.LastName ?? string.Empty,
                FatherName: user.FatherName,
                Email: user.Email ?? string.Empty,
                StudentId: studentId,
                TeacherId: teacherId,
                GroupId: groupId,
                GroupName: groupName);
        }
    }
}
