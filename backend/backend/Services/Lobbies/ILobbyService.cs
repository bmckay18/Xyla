using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public interface ILobbyService
    {
        LobbyDto CreateLobby(string host, string? password);
        LobbyDetailsDto? GetLobby(Guid id);
    }
}