using Backend.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(Guid userId, string password);
    }
}
