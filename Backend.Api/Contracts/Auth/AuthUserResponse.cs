using System.Diagnostics;

namespace Backend.Api.Contracts.Auth
{
    public sealed record AuthUserResponse(
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
