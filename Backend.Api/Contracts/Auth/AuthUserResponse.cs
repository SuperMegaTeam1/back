using System.Diagnostics;

namespace Backend.Api.Contracts.Auth
{
    public sealed record AuthUserResponse(
            Guid Id,
            Guid RoleId,
            string FirstName,
            string LastName,
            string? FatherName,
            string Email,
            int UserType
        );
}
