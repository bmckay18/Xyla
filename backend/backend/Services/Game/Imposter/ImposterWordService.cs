using System.Text.Json;

namespace Backend.Services.Game.Imposter
{
    public record WordCombination(string word, string hint);

    public class ImposterWordService : IImposterWordService
    {
        private const string _wordListFilePath = "wordList.json";

        public async Task<WordCombination> GetRandomWord()
        {
            var wordList = await GetWordList();

            var randomSelector = new Random();

            var wordIndex = randomSelector.Next(wordList.Count());

            return wordList[wordIndex];
        }

        private async Task<List<WordCombination>> GetWordList()
        {
            using (StreamReader reader = new StreamReader(_wordListFilePath))
            {
                var jsonContent = reader.ReadToEnd();

                var wordList = JsonSerializer.Deserialize<List<WordCombination>>(jsonContent);

                return wordList;
            }
        }
    }
}
