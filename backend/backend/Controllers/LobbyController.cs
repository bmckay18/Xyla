using Backend.Controllers.Models;
using Backend.Services.Lobbies;
using Backend.Services.Lobbies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize]
        public ActionResult<LobbyDetailsDto> GetLobby()
        {
            var playerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(playerIdClaim, out var playerId))
            {
                return Unauthorized();
            }

            var lobby = _lobbyService.GetLobby(playerId);

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