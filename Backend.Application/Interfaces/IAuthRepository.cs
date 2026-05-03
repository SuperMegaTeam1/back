using Backend.Application.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IAuthRepository
    {
        // todo переименовать в userRepo
        Task<AuthUser?> FindByEmailAsync(string email);
        Task<AuthUser?> FindByIdAsync(Guid userId);
        Task<bool> CheckPasswordAsync(Guid userId, string password);
    }
}
