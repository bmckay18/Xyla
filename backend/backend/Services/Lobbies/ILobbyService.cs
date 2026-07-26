using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public interface ILobbyService
    {
        Lobby CreateLobby(string host);
        Lobby? GetLobby(Guid id);
    }
}