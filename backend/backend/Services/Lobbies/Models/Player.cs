namespace Backend.Services.Lobbies.Models
{
    public class Player
    {
        public Guid Id { get; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? ConnectionId { get; set; }
    }
}
