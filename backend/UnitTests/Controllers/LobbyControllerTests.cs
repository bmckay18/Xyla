using Backend.Controllers;
using Backend.Controllers.Models;
using Backend.Services.Lobbies;
using Backend.Services.Lobbies.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers
{
    public class LobbyControllerTests
    {
        private Mock<ILobbyService> _lobbyServiceMock;
        private LobbyController _controller;

        [SetUp]
        public void Setup()
        {
            _lobbyServiceMock = new Mock<ILobbyService>();

            _controller = new LobbyController(_lobbyServiceMock.Object);
        }

        [Test]
        public void CreateLobby_CallsLobbyServiceCreateLobbyMethod_WithCorrectParameters()
        {
            var request = new CreateLobbyRequest
            {
                HostName = "user1",
                Password = "password"
            };

            _controller.CreateLobby(request);

            _lobbyServiceMock.Verify(s => s.CreateLobby(request.HostName, request.Password), Times.Once);
        }

        [Test]
        public void GetLobby_ReturnsOkResult_WhenLobbyExists()
        {
            var lobbyId = Guid.NewGuid();

            var mockPlayer = new Player { Name = "User1" };

            var mockResult = new LobbyDetailsDto
            {
                HostId = "1",
                Players = new List<Player> { mockPlayer }
            }; 

            _lobbyServiceMock.Setup(r => r.GetLobby(It.Is<Guid>(g => g == lobbyId), It.Is<Guid>(g => g == mockPlayer.Id))).Returns(mockResult);

            var result = _controller.GetLobby(lobbyId, mockPlayer.Id);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            _lobbyServiceMock.Verify(r => r.GetLobby(It.Is<Guid>(g => g == lobbyId), It.Is<Guid>(g => g == mockPlayer.Id)), Times.Once);
            Assert.That(okResult!.Value, Is.EqualTo(mockResult));
        }

        [Test]
        public void GetLobby_ReturnsNotFoundResult_WhenLobbyDoesNotExist()
        {
            var lobbyId = Guid.NewGuid();

            _lobbyServiceMock.Setup(r => r.GetLobby(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((LobbyDetailsDto?)null);

            var result = _controller.GetLobby(lobbyId, Guid.Empty);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
            _lobbyServiceMock.Verify(r => r.GetLobby(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }        
    }
}
