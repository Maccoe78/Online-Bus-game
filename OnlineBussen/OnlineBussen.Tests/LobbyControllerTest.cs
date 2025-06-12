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

        [Fact]
        public async Task GetLobbyByName_ReturnsLobby()
        {
            var lobby = new Lobby { LobbyName = "testlobby", LobbyPassword = "testpass" };
            _mockLobbyService
                .Setup(x => x.GetLobbyByNameAsync("testlobby"))
                .ReturnsAsync(lobby);

            var result = await _controller.GetLobbyByNameAsync("testlobby");

            Assert.NotNull(result);
            Assert.Equal("testlobby", result.LobbyName);
        }

        [Fact]
        public async Task GetLobbyByName_DoesNotReturnLobby()
        {
            var lobby = new Lobby { LobbyName = "testlobby", LobbyPassword = "testpass" };
            _mockLobbyService
                .Setup(x => x.GetLobbyByNameAsync("testlobby"))
                .ReturnsAsync(lobby);

            var result = await _controller.GetLobbyByNameAsync("wronglobby");

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteLobby_ReturnsSuccesful()
        {
            var lobbyId = 1;
            _mockLobbyService
                .Setup(x => x.DeleteLobbyAsync(lobbyId))
                .Returns(Task.CompletedTask);

            await _controller.DeleteLobbyAsync(lobbyId);

            _mockLobbyService.Verify(x => x.DeleteLobbyAsync(lobbyId), Times.Once);
        }

        
        [Fact]
        public async Task AddPlayerToLobby_ReturnsSuccesful()
        {
            _mockLobbyService
                .Setup(x => x.AddPlayerToLobbyAsync(1, "username"))
                .Returns(Task.CompletedTask);

            await _controller.AddPlayerToLobbyAsync(1, "username");

            _mockLobbyService.Verify(x => x.AddPlayerToLobbyAsync(1, "username"), Times.Once);
        }

        [Fact]
        public async Task IsPlayerInLobby_ReturnsTrue()
        {
            _mockLobbyService
                .Setup(x => x.IsPlayerInLobbyAsync(1, "username"))
                .ReturnsAsync(true);

            var result = await _controller.IsPlayerInLobbyAsync(1, "username");

            Assert.True(result);
        }

        [Fact]
        public async Task IsPlayerInLobby_ReturnsFalse()
        {
            _mockLobbyService
                .Setup(x => x.IsPlayerInLobbyAsync(1, "username"))
                .ReturnsAsync(true);

            var result = await _controller.IsPlayerInLobbyAsync(1, "wrongUsername");

            Assert.False(result);
        }

        [Fact]
        public async Task RemovePlayerFromLobby_ReturnsTrue()
        {
            _mockLobbyService
                .Setup(x => x.RemovePlayerFromLobbyAsync(1, "username"))
                .Returns(Task.CompletedTask);

            await _controller.RemovePlayerFromLobbyAsync(1, "username");

            _mockLobbyService.Verify(x => x.RemovePlayerFromLobbyAsync(1, "username"), Times.Once);
        }
    }
}
