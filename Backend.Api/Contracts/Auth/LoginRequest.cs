using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Contracts.Auth
{
    public sealed class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        [Required]
        public string Password { get; init; }

        public LoginRequest(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
