using AutoMapper;
using Backend.Services.Lobbies.Models;

namespace Backend.Services.Lobbies
{
    public class LobbyService : ILobbyService
    {
        private readonly IMapper _mapper;
        private readonly List<Lobby> _lobbies = new();    
        public LobbyService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public LobbyDto CreateLobby(string hostName, string? password)
        {
            var player = new Player
            {
                Name = hostName
            };

            var passwordDetails = string.IsNullOrWhiteSpace(password)
                ? null
                : GenerateLobbyPassword(password);

            var lobby = new Lobby
            {
                Host = player,
                PasswordDetails = passwordDetails
            };

            lobby.Players.Add(player);

            _lobbies.Add(lobby);

            return new LobbyDto
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id
            };
        }

        public LobbyDetailsDto? GetLobby(Guid id)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Id == id);

            if (lobby is null)
            {
                return null;
            }

            return _mapper.Map<LobbyDetailsDto>(lobby);
        }

        private static PasswordDetails GenerateLobbyPassword(string password)
        {
            var salt = LobbyPasswordService.GenerateSalt();
            var passwordHash = LobbyPasswordService.HashPassword(password, salt);

            return new PasswordDetails { Password = passwordHash, Salt = salt };
        }
    }
}
