using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public interface ILobbyService
    {
        LobbyDto CreateLobby(string host, string? password);
        LobbyDetailsDto? GetLobby(Guid playerId);
        Guid? GetLobbyId(Guid playerId);
        Task<LobbyDto?> JoinLobby(string displayName, Guid lobbyId, string? password);
        Task KickPlayer(Guid hostPlayerId, Guid kickPlayerId);
        Task LeaveLobby(Guid playerId);
        Task UpdateConnectionId(Guid playerId, string connectionId);
    }
}