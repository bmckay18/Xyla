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
        [Route("{lobbyId}")]
        public ActionResult<LobbyDetailsDto> GetLobby(Guid lobbyId)
        {
            var lobby = _lobbyService.GetLobby(lobbyId);

            if (lobby is null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }
    }
}