using Backend.Hubs;
using Backend.Services.Lobbies.Models;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Services.Notifier
{
    public class NotifierService : INotifierService
    {
        private readonly IHubContext<GameHub> _hub;

        public NotifierService(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

        public async Task SendNotification(Guid lobbyId, string messageType, object messageData)
        {
            await _hub.Clients.Group(lobbyId.ToString())
                .SendAsync(messageType, messageData);
        }

        public async Task PlayerJoined(Guid lobbyId, Player player)
        {
            await SendNotification(lobbyId, "PlayerJoined", player);
        }

        public async Task PlayerLeft(Guid lobbyId, Player player)
        {
            await SendNotification(lobbyId, "PlayerLeft", player);
        }
    }
}
