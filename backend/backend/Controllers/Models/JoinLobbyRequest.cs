namespace Backend.Controllers.Models
{
    public class JoinLobbyRequest
    {
        public string DisplayName { get; set; }
        public Guid LobbyId { get; set; }
        public string? Password { get; set; }
    }
}
