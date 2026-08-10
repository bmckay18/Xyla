using Backend.Services.Lobbies.Models;

namespace Backend.Services.Notifier
{
    public interface INotifierService
    {
        Task PlayerJoined(Guid lobbyId, Player player);
        Task PlayerLeft(Guid lobbyId, Player player);
        Task SendNotification(Guid lobbyId, string messageType, object messageData);
    }
}