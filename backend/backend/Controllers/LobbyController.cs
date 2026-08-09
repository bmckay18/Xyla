using Backend.Controllers.Models;
using Backend.Services.Lobbies;
using Backend.Services.Lobbies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/lobbies")]
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        [HttpPost]
        public ActionResult<LobbyDto> CreateLobby(CreateLobbyRequest request)
        {
            var lobby = _lobbyService.CreateLobby(request.HostName, request.Password);

            return Ok(lobby);
        }

        [HttpGet]
        [Route("{lobbyId}/{playerId}")]
        public ActionResult<LobbyDetailsDto> GetLobby(Guid lobbyId, Guid playerId)
        {
            var lobby = _lobbyService.GetLobby(lobbyId, playerId);

            if (lobby is null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }

        [HttpPost]
        [Route("join")]
        public async Task<ActionResult<LobbyDto>> JoinLobbyAsync(JoinLobbyRequest request)
        {
            var lobby = await _lobbyService.JoinLobby(request.DisplayName, request.LobbyId, request.Password);

            if (lobby is null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }
    }
}