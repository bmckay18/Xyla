namespace Backend.Services.Lobbies.Models
{
    public class LobbyDto
    {
        public Guid LobbyId { get; set; }
        public Guid PlayerId { get; set; }
        public required string Jwt { get; set; }
    }
}
