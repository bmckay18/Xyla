using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public interface ILobbyService
    {
        LobbyDto CreateLobby(string host, string? password);
        LobbyDetailsDto? GetLobby(Guid lobbyId, Guid playerId);
        Guid? GetLobbyId(Guid playerId);
        Task<LobbyDto?> JoinLobby(string displayName, Guid lobbyId, string? password);
        Task LeaveLobby(Guid playerId);
    }
}