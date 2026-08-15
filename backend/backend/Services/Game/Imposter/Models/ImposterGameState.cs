namespace Backend.Services.Game.Imposter.Models
{
    public class ImposterGameState
    {
        public string? Word { get; set; }
        public Guid? ImposterId { get; set; }
        public string? ImposterHint { get; set; }
    }
}
