using AutoMapper;
using Backend.CustomExceptions;
using Backend.Services.Lobbies.Models;
using Backend.Services.Notifier;
using Backend.Services.Token;

namespace Backend.Services.Lobbies
{
    public class LobbyService : ILobbyService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly INotifierService _notifier;

        private readonly List<Lobby> _lobbies = new();

        public LobbyService(IMapper mapper, ITokenService tokenService, INotifierService notifier)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _notifier = notifier;
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

            var jwt = _tokenService.GenerateToken(player.Id);

            return new LobbyDto
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id,
                Jwt = jwt
            };
        }

        public LobbyDetailsDto? GetLobby(Guid playerId)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Players.Any(p => p.Id == playerId));

            if (lobby is null)
            {
                return null;
            }

            return _mapper.Map<LobbyDetailsDto>(lobby);
        }

        public Guid? GetLobbyId(Guid playerId)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Players.Any(p => p.Id == playerId));

            if (lobby is null)
            {
                return null;
            }

            return lobby.Id;
        }

        public async Task<LobbyDto?> JoinLobby(string displayName, Guid lobbyId, string? password)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);

            var canJoinLobby = CanJoinLobby(displayName, lobbyId, password);

            if (!canJoinLobby.Status)
            {
                throw new BadRequestException(canJoinLobby.Message ?? "An error occurred.");
            }

            var player = new Player
            {
                Name = displayName
            };

            lobby!.Players.Add(player);

            var jwt = _tokenService.GenerateToken(player.Id);

            await _notifier.PlayerJoined(lobbyId, player);

            return new LobbyDto
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id,
                Jwt = jwt
            };
        }

        public async Task LeaveLobby(Guid playerId)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Players.Any(p => p.Id == playerId));

            if (lobby is null)
            {
                return;
            }

            var player = lobby.Players.First(x => x.Id == playerId);

            lobby.Players.Remove(player);

            await _notifier.PlayerLeft(lobby.Id, player);
        }
        
        public async Task KickPlayer(Guid hostPlayerId, Guid kickPlayerId)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Players.Any(p => p.Id == hostPlayerId));

            if (lobby is null)
            {
                return;
            }

            if (lobby.Host.Id != hostPlayerId)
            {
                throw new ForbiddenException("Only the host can kick players.");
            }

            await LeaveLobby(kickPlayerId); // After this, need to disconnect kicked player from the hub
        }

        private CanJoinLobbyDetails CanJoinLobby(string name, Guid lobbyId, string? password)
        {
            var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);

            if (lobby is null)
            {
                return new CanJoinLobbyDetails(false, "The lobby does not exist.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return new CanJoinLobbyDetails(false, "Your display name cannot be empty");
            }

            if (lobby.Players.Any(p => p.Name == name))
            {
                return new CanJoinLobbyDetails(false, "Your display name must be unique");
            }

            if (lobby.PasswordDetails is not null)
            {
                var isCorrectPassword = LobbyPasswordService.ValidatePassword(password ?? string.Empty, lobby.PasswordDetails.Password, lobby.PasswordDetails.Salt);

                if (!isCorrectPassword)
                {
                    return new CanJoinLobbyDetails(false, "Invalid password.");
                }
            }

            return new CanJoinLobbyDetails(true, null);
        }

        private static PasswordDetails GenerateLobbyPassword(string password)
        {
            var salt = LobbyPasswordService.GenerateSalt();
            var passwordHash = LobbyPasswordService.HashPassword(password, salt);

            return new PasswordDetails { Password = passwordHash, Salt = salt };
        }
    }
}
