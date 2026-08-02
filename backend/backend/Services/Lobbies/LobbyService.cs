using AutoMapper;
using Backend.CustomExceptions;
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

        public LobbyDetailsDto? GetLobby(Guid lobbyId, Guid playerId)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId && x.Players.Any(p => p.Id == playerId));

            if (lobby is null)
            {
                return null;
            }

            return _mapper.Map<LobbyDetailsDto>(lobby);
        }

        public LobbyDto? JoinLobby(string displayName, Guid lobbyId, string? password)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);

            if (lobby is null)
            {
                return null;
            }

            var existingUsername = lobby.Players.Any(p => p.Name == displayName);

            if (existingUsername)
            {
                throw new BadRequestException("Display name must be unique.");
            }

            if (lobby.PasswordDetails is not null)
            {
                var isCorrectPassword = LobbyPasswordService.ValidatePassword(password ?? string.Empty, lobby.PasswordDetails.Password, lobby.PasswordDetails.Salt);

                if (!isCorrectPassword)
                {
                    throw new UnauthorisedException("Invalid password");
                }
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                return null;
            }

            var player = new Player
            {
                Name = displayName
            };

            lobby.Players.Add(player);

            return new LobbyDto
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id
            };
        }

        private static PasswordDetails GenerateLobbyPassword(string password)
        {
            var salt = LobbyPasswordService.GenerateSalt();
            var passwordHash = LobbyPasswordService.HashPassword(password, salt);

            return new PasswordDetails { Password = passwordHash, Salt = salt };
        }
    }
}
