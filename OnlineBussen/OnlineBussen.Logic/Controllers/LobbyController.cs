using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Logic.Controllers
{
    public class LobbyController
    {
        private readonly ILobbyService _lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public async Task<(bool success, string message)> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username)
        {
            if (await _lobbyService.LobbyNameExistsAsync(lobbyName))
            {
                return (false, "A lobby with this name already exists");
            }

            await _lobbyService.CreateLobbyAsync(lobbyName, lobbyPassword, username);
            return (true, "Lobby created successfully");
        }
        public async Task DeleteLobbyAsync(int lobbyId)
        {
            await _lobbyService.DeleteLobbyAsync(lobbyId);
        }
        public async Task<Lobby> GetLobbyByNameAsync(string lobbyName)
        {
            return await _lobbyService.GetLobbyByNameAsync(lobbyName);
        }
        public async Task<Lobby> GetLobbyByIdAsync(int lobbyId)
        {
            return await _lobbyService.GetLobbyByIdAsync(lobbyId);
        }

        public async Task<IEnumerable<Lobby>> GetAllLobbiesAsync()
        {
            return await _lobbyService.GetAllLobbiesAsync();
        }
        public async Task UpdateLobbyStatusAsync(int lobbyId, string status)
        {
            await _lobbyService.UpdateLobbyStatusAsync(lobbyId, status);
        }
        public async Task AddPlayerToLobbyAsync(int lobbyId, string username)
        {
            await _lobbyService.AddPlayerToLobbyAsync(lobbyId, username);
        }
        public async Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username)
        {
            return await _lobbyService.IsPlayerInLobbyAsync(lobbyId, username);
        }
        public async Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername)
        {
            await _lobbyService.UpdatePlayerUsernameInLobbiesAsync(oldUsername, newUsername);
        }
        public async Task RemovePlayerFromLobbyAsync(int lobbyId, string username)
        {
            await _lobbyService.RemovePlayerFromLobbyAsync(lobbyId, username);
        }
        public async Task<int?> GetUserActiveLobbyIdAsync(string username)
        {
            return await _lobbyService.GetUserActiveLobbyIdAsync(username);
        }

    }
}
