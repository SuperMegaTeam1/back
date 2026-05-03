using System.Diagnostics;

namespace Backend.Api.Contracts.Auth
{
    public sealed record AuthStudentResponse(
            Guid Id,
            string RoleName,
            string FirstName,
            string LastName,
            string? FatherName,
            string Email,
            Guid? StudentId,
            Guid? GroupId,
            string? GroupName
        );
}
