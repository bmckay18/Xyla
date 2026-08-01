using AutoMapper;
using Backend.Mapping;
using Backend.Services.Lobbies;
using Microsoft.Extensions.Logging.Abstractions;

namespace UnitTests.Services
{
    [TestFixture]
    public class LobbyServiceTests
    {
        private ILobbyService _service;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LobbyMappingProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = mapperConfig.CreateMapper();

            _service = new LobbyService(_mapper);
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
    }
}
