using Backend.Services.Lobbies.Models;

namespace Backend.Services.Notifier
{
    public interface INotifierService
    {
        Task PlayerJoined(Guid lobbyId, Player player);
        Task PlayerLeft(Guid lobbyId, Player player);
        Task SendClientNotificationAsync(string connectionId, string messageType, object? messageData);
        Task SendLobbyNotificationAsync(Guid lobbyId, string messageType, object? messageData);
    }
}