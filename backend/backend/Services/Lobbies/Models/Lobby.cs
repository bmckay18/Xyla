namespace Backend.Services.Lobbies.Models
{
    public class Lobby
    {
        public Guid Id { get; } = Guid.NewGuid();
        public required Player Host { get; set; }
        public List<Player> Players { get; set; } = new();
        public PasswordDetails? PasswordDetails { get; set; }
    }
}
