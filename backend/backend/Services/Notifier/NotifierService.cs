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

        public async Task SendLobbyNotificationAsync(Guid lobbyId, string messageType, object? messageData)
        {
            if (messageData is not null)
            {
                await _hub.Clients.Group(lobbyId.ToString())
                    .SendAsync(messageType, messageData);
            }

            await _hub.Clients.Group(lobbyId.ToString())
                    .SendAsync(messageType);
        }

        public async Task SendClientNotificationAsync(string connectionId, string messageType, object? messageData)
        {
            if (messageData is not null)
            {
                await _hub.Clients.Client(connectionId)
                    .SendAsync(messageType, messageData);
            }

            await _hub.Clients.Client(connectionId)
                    .SendAsync(messageType);
        }

        public async Task PlayerJoined(Guid lobbyId, Player player)
        {
            await SendLobbyNotificationAsync(lobbyId, "PlayerJoined", player);
        }

        public async Task PlayerLeft(Guid lobbyId, Player player)
        {
            await SendLobbyNotificationAsync(lobbyId, "PlayerLeft", player);
        }
    }
}
