using Backend.Services.Lobbies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ILobbyService _lobbyService;

        public GameHub(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public async Task JoinLobby()
        {
            var playerId = Context.UserIdentifier;

            if (!Guid.TryParse(playerId, out Guid id))
            {
                throw new HubException("Invalid player identity.");
            }

            var lobbyId = _lobbyService.GetLobbyId(id);

            if (lobbyId is null)
            {
                throw new HubException("No lobby found.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId.ToString()!);
        }

        public async Task LeaveLobby(string lobbyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = Context.UserIdentifier;

            if (!Guid.TryParse(playerId, out Guid id))
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await _lobbyService.LeaveLobby(id);
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            var playerId = Context.UserIdentifier;

            if (!Guid.TryParse(playerId, out Guid id))
            {
                Context.Abort();
                return;
            }

            await _lobbyService.UpdateConnectionId(id, Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}
