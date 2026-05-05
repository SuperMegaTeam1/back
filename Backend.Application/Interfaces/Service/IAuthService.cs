using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Auth;

namespace Backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult?> LoginAsync(string email, string password);
        Task<AuthUserResult?> GetByIdAsync(Guid userId);
    }
}
