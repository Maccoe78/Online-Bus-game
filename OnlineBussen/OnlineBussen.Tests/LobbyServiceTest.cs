using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using OnlineBussen.Logic.Services;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Data.Interfaces;
using OnlineBussen.Logic.Models;
using OnlineBussen.Data.Models;

namespace OnlineBussen.Tests
{
    public class LobbyServiceTest
    {
        private readonly Mock<ILobbyRepository> _mockLobbyRepository;
        private readonly LobbyService _lobbyService;

        public LobbyServiceTest()
        {
            _mockLobbyRepository = new Mock<ILobbyRepository>();
            _lobbyService = new LobbyService(_mockLobbyRepository.Object);
        }

        [Fact]
        public async Task CreateLobbyAsync_CreatesLobby_Successfully()
        {
            // Arrange
            var lobbyName = "TestLobby";
            var lobbyPassword = "password123";
            var username = "testuser";
            var expectedLobbyId = 1;

            _mockLobbyRepository
                .Setup(x => x.CreateLobbyAsync(lobbyName, lobbyPassword, username))
                .ReturnsAsync(expectedLobbyId);

            // Act
            await _lobbyService.CreateLobbyAsync(lobbyName, lobbyPassword, username);

            // Assert
            _mockLobbyRepository.Verify(x => x.CreateLobbyAsync(lobbyName, lobbyPassword, username), Times.Once);
        }

        [Fact]
        public async Task LobbyNameExistsAsync_ReturnsTrue_WhenLobbyExists()
        {
            // Arrange
            _mockLobbyRepository
                .Setup(x => x.LobbyNameExistsAsync("ExistingLobby")) 
                .ReturnsAsync(true);  

            // Act
            var result = await _lobbyService.LobbyNameExistsAsync("ExistingLobby");

            // Assert
            Assert.True(result);
            _mockLobbyRepository.Verify(x => x.LobbyNameExistsAsync("ExistingLobby"), Times.Once);
        }


        [Fact]
        public async Task GetLobbyByIdAsync_ReturnsLobby_WhenExists()
        {
            // Arrange
            var lobby = new LobbyDTO
            {
                LobbyId = 1,
                LobbyName = "TestLobby",
                LobbyPassword = "password"
            };

            _mockLobbyRepository
                .Setup(x => x.GetLobbyByIdAsync(1))
                .ReturnsAsync(lobby);

            // Act
            var result = await _lobbyService.GetLobbyByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.LobbyId);
            Assert.Equal("TestLobby", result.LobbyName);
        }

        [Fact]
        public async Task AddPlayerToLobbyAsync_AddsPlayer_Successfully()
        {
            // Arrange
            var lobbyId = 1;
            var username = "testplayer";

            _mockLobbyRepository
                .Setup(x => x.AddPlayerToLobbyAsync(lobbyId, username))
                .Returns(Task.CompletedTask);

            // Act
            await _lobbyService.AddPlayerToLobbyAsync(lobbyId, username);

            // Assert
            _mockLobbyRepository.Verify(x => x.AddPlayerToLobbyAsync(lobbyId, username), Times.Once);
        }

        [Fact]
        public async Task RemovePlayerFromLobbyAsync_RemovesPlayer_Successfully()
        {
            // Arrange
            var lobbyId = 1;
            var username = "testplayer";

            _mockLobbyRepository
                .Setup(x => x.RemovePlayerFromLobbyAsync(lobbyId, username))
                .Returns(Task.CompletedTask);

            // Act
            await _lobbyService.RemovePlayerFromLobbyAsync(lobbyId, username);

            // Assert
            _mockLobbyRepository.Verify(x => x.RemovePlayerFromLobbyAsync(lobbyId, username), Times.Once);
        }

        [Fact]
        public async Task IsPlayerInLobbyAsync_ReturnsTrue_WhenPlayerInLobby()
        {
            // Arrange
            _mockLobbyRepository
                .Setup(x => x.IsPlayerInLobbyAsync(1, "testplayer"))
                .ReturnsAsync(true);

            // Act
            var result = await _lobbyService.IsPlayerInLobbyAsync(1, "testplayer");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsPlayerInLobbyAsync_ReturnsFalse_WhenPlayerNotInLobby()
        {
            // Arrange
            _mockLobbyRepository
                .Setup(x => x.IsPlayerInLobbyAsync(1, "notinlobby"))
                .ReturnsAsync(false);

            // Act
            var result = await _lobbyService.IsPlayerInLobbyAsync(1, "notinlobby");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteLobbyAsync_DeletesLobby_Successfully()
        {
            // Arrange
            var lobbyId = 1;

            _mockLobbyRepository
                .Setup(x => x.DeleteLobbyAsync(lobbyId))
                .Returns(Task.CompletedTask);

            // Act
            await _lobbyService.DeleteLobbyAsync(lobbyId);

            // Assert
            _mockLobbyRepository.Verify(x => x.DeleteLobbyAsync(lobbyId), Times.Once);
        }

        [Fact]
        public async Task GetAllLobbiesAsync_ReturnsAllLobbies()
        {
            // Arrange
            var lobbies = new List<LobbyDTO>
            {
                new LobbyDTO { LobbyId = 1, LobbyName = "Lobby1" },
                new LobbyDTO { LobbyId = 2, LobbyName = "Lobby2" }
            };

            _mockLobbyRepository
                .Setup(x => x.GetAllLobbiesAsync())
                .ReturnsAsync(lobbies);

            // Act
            var result = await _lobbyService.GetAllLobbiesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, l => l.LobbyName == "Lobby1");
            Assert.Contains(result, l => l.LobbyName == "Lobby2");
        }

        [Fact]
        public async Task GetLobbyByNameAsync_ReturnsCorrectLobby()
        {
            // Arrange
            var lobby = new LobbyDTO 
            { 
                LobbyId = 1, 
                LobbyName = "SpecificLobby", 
                LobbyPassword = "secret" 
            };

            _mockLobbyRepository
                .Setup(x => x.GetLobbyByNameAsync("SpecificLobby"))
                .ReturnsAsync(lobby);

            // Act
            var result = await _lobbyService.GetLobbyByNameAsync("SpecificLobby");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SpecificLobby", result.LobbyName);
            Assert.Equal("secret", result.LobbyPassword);
        }
    }
}