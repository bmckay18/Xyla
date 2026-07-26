namespace Backend.Services.Lobbies.Models
{
    public class Player
    {
        public Guid Id { get; } = new();
        public required string Name { get; set; }
    }
}
