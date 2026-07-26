using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public class LobbyService : ILobbyService
    {
        private readonly List<Lobby> _lobbies = new();

        public Lobby CreateLobby(string hostName)
        {
            var player = new Player
            {
                Name = hostName
            };

            var lobby = new Lobby
            {
                Host = player
            };

            lobby.Players.Add(player);

            _lobbies.Add(lobby);

            return lobby;
        }

        public Lobby? GetLobby(Guid id)
        {
            return _lobbies.FirstOrDefault(x => x.Id == id);
        }
    }
}
