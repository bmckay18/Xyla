namespace Backend.Services.Lobbies.Models
{
    public class LobbyDetailsDto
    {
        public List<Player> Players { get; set; }
        public required string HostId { get; set; }
    }
}
