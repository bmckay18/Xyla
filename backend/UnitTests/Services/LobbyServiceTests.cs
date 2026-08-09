using AutoMapper;
using Backend.CustomExceptions;
using Backend.Hubs;
using Backend.Mapping;
using Backend.Services.Lobbies;
using Backend.Services.Token;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace UnitTests.Services
{
    [TestFixture]
    public class LobbyServiceTests
    {
        private ILobbyService _service;
        private IMapper _mapper;
        private Mock<IHubContext<GameHub>> _hubMock;
        private Mock<ITokenService> _tokenServiceMock;

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LobbyMappingProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = mapperConfig.CreateMapper();
            _hubMock = new Mock<IHubContext<GameHub>>();
            _tokenServiceMock = new Mock<ITokenService>();

            _service = new LobbyService(_mapper, _hubMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public void CreateLobby_AssignsValidGuidsToLobbyAndPlayer_WhenValidDataIsPresent()
        {
            var hostName = "user1";
            var password = "test123";

            var lobby = _service.CreateLobby(hostName, password);

            Assert.That(lobby, Is.Not.Null);
            Assert.That(lobby.LobbyId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(lobby.PlayerId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void CreateLobby_CreatesAndStoresLobbyInMemory()
        {
            var hostName = "user3";

            var lobby = _service.CreateLobby(hostName, null);
            var retrievedLobby = _service.GetLobby(lobby.LobbyId, lobby.PlayerId);

            Assert.That(lobby, Is.Not.Null);
            Assert.That(retrievedLobby, Is.Not.Null);
            Assert.That(lobby.PlayerId.ToString(), Is.EqualTo(retrievedLobby.HostId));
            Assert.That(retrievedLobby.Players.Count, Is.EqualTo(1));
            Assert.That(retrievedLobby.Players[0].Name, Is.EqualTo(hostName));
        }

        [Test]
        public async Task JoinLobby_SuccessfullyAddsUser()
        {
            var userName = "user 123";

            var lobby = _service.CreateLobby(userName, null);

            var retrievedLobby = await _service.JoinLobby("joined user", lobby.LobbyId, null);
            var newLobbyData = _service.GetLobby(lobby.LobbyId, lobby.PlayerId);

            Assert.That(retrievedLobby, Is.Not.Null);
            Assert.That(retrievedLobby.LobbyId, Is.EqualTo(lobby.LobbyId));
            Assert.That(retrievedLobby.PlayerId, Is.TypeOf<Guid>());
            Assert.That(retrievedLobby.PlayerId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(newLobbyData?.Players.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task JoinLobby_AddsUserToLobby_WhenCorrectPasswordProvided()
        {
            var userName = "user 123";
            var password = "abc123";

            var lobby = _service.CreateLobby(userName, password);

            var retrievedLobby = await _service.JoinLobby("joined user", lobby.LobbyId, password);

            Assert.That(retrievedLobby, Is.Not.Null);
            Assert.That(retrievedLobby.PlayerId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void JoinLobby_ThrowsBadRequestException_WhenWrongPasswordSupplied()
        {
            var userName = "user 123";
            var password = "abc123";

            var lobby = _service.CreateLobby(userName, password);

            Assert.Throws<BadRequestException>(() =>
            {
                _service.JoinLobby("joined user", lobby.LobbyId, "an invalid password");
            });
        }

        [Test]
        public async Task JoinLobby_AddsUserToLobby_WhenPasswordProvidedForNonPasswordLobby()
        {
            var userName = "user 123";

            var lobby = _service.CreateLobby(userName, null);

            var retrievedLobby = await _service.JoinLobby("joined user", lobby.LobbyId, "a password");

            Assert.That(retrievedLobby, Is.Not.Null);
            Assert.That(retrievedLobby.PlayerId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void JoinLobby_ThrowsBadRequestException_WhenUserAttemptsToJoinWithSameUsernameAsExistingPlayer()
        {
            var userName = "user 123";

            var lobby = _service.CreateLobby(userName, null);

            Assert.Throws<BadRequestException>(() =>
            {
                _service.JoinLobby(userName, lobby.LobbyId, null);
            });
        }

        [Test]
        public async Task GetLobbyId_ReturnsLobbyId_ForValidPlayer()
        {
            var userName = "user 123";

            var lobby = _service.CreateLobby(userName, null);

            var lobbyId = _service.GetLobbyId(lobby.PlayerId);

            Assert.That(lobbyId, Is.EqualTo(lobby.LobbyId));
        }

        [Test]
        public async Task GetLobbyId_ReturnsNull_WhenPlayerNotLinkedToALobby()
        {
            var lobbyId = _service.GetLobbyId(Guid.Empty);

            Assert.That(lobbyId, Is.Null);
        }
    }
}
