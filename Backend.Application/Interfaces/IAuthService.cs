using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models;

namespace Backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult?> LoginAsync(string email, string password);
    }
}
