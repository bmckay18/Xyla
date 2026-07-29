namespace Backend.Services.Lobbies.Models
{
    public class PasswordDetails
    {
        public required string Password { get; init; }
        public required byte[] Salt { get; init; }
    }
}
