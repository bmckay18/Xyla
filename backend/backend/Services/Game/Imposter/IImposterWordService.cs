namespace Backend.Services.Game.Imposter
{
    public interface IImposterWordService
    {
        Task<WordCombination> GetRandomWord();
    }
}