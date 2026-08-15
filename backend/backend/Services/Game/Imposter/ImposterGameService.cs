using Backend.Services.Game.Imposter.Models;
using Backend.Services.Lobbies;

namespace Backend.Services.Game.Imposter
{
    public class ImposterGameService : IGameService
    {
        private readonly ILobbyService _lobbyService;
        private readonly IImposterWordService _wordService;

        private readonly Guid _lobbyId;
        private ImposterGameState _gameState = new();

        public ImposterGameService(ILobbyService lobbyService, IImposterWordService wordService, Guid lobbyId)
        {
            _lobbyService = lobbyService;
            _lobbyId = lobbyId;
            _wordService = wordService;
        }

        public async Task Start()
        {
            var lobby = _lobbyService.GetLobby(_lobbyId);

            if (lobby is null)
            {
                throw new ArgumentException("The lobby ID does not correspond to any existing lobbies.");
            }

            var randomGenerator = new Random();

            var playerCount = lobby.Players.Count();

            var imposterPosition = randomGenerator.Next(playerCount);

            _gameState.ImposterId = lobby.Players[imposterPosition].Id;

            var randomWord = await _wordService.GetRandomWord();
        }
    }
}
