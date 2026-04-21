namespace Backend.Api.Contracts.Auth
{
    public sealed class LoginRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public LoginRequest(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
