namespace Backend.Services.Lobbies.Models
{
    public class LobbyDto
    {
        public Guid Id { get; set; }
        public required Player Host { get; set; }
        public required List<Player> Players { get; set; }
        public Player? CurrentPlayer { get; set; }
    }
}
