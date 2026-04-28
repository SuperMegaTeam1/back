using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models
{
    public sealed record AuthUser(
        Guid Id,
        string RoleName,
        string FirstName,
        string LastName,
        string? FatherName,
        string Email,
        Guid? StudentId,
        Guid? TeacherId,
        Guid? GroupId,
        string? GroupName
    );
}
