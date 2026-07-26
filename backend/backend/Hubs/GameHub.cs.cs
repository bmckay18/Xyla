using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinLobby(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }
    }
}
