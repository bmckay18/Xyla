namespace Backend.Controllers.Models
{
    public class CreateLobbyRequest
    {
        public required string HostName { get; set; }
        public string? Password { get; set; }
    }
}
