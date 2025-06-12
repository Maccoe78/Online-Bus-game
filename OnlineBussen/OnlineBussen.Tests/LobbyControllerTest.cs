using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Tests
{
    public class LobbyControllerTest
    {
        private readonly Mock<ILobbyService> _mockLobbyService;
        private readonly LobbyController _controller;

        public LobbyControllerTest()
        {
            _mockLobbyService = new Mock<ILobbyService>();
            _controller = new LobbyController(_mockLobbyService.Object);
        }

        [Fact]
        public async Task CreateLobby_ReturnsSucces()
        {

            _mockLobbyService
                .Setup(x => x.LobbyNameExistsAsync("existinglobby"))
                .ReturnsAsync(true);

            _mockLobbyService
                .Setup(x => x.LobbyNameExistsAsync("nonlobby"))
                .ReturnsAsync(false);


            var result = await _controller.CreateLobbyAsync("nonlobby", "lobbypass", "username");

            Assert.True(result.success);
            Assert.Equal("Lobby created successfully", result.message);
        }

        [Fact]
        public async Task CreateLobby_ReturnFalse_ExistingLobbyName()
        {
            _mockLobbyService
                .Setup(x => x.LobbyNameExistsAsync("existinglobby"))
                .ReturnsAsync(true);
            _mockLobbyService
                .Setup(x => x.LobbyNameExistsAsync("nonlobby"))
                .ReturnsAsync(false);

            var result = await _controller.CreateLobbyAsync("existinglobby", "lobbypass", "username");

            Assert.False(result.success);
            Assert.Equal("A lobby with this name already exists", result.message);
        }

    }
}
