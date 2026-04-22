using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models
{
    public sealed record AuthUserResult(
        Guid Id,
        Guid RoleId,
        string FirstName,
        string LastName,
        string? FatherName,
        string Email,
        string RoleName
    );
}
