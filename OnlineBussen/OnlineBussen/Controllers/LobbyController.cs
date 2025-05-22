using OnlineBussen.Interfaces;
using OnlineBussen.Models;

namespace OnlineBussen.Controllers
{
    public class LobbyController
    {
        private readonly ILobbyRepository _lobbyRepository;

        public LobbyController(ILobbyRepository lobbyRepository)
        {
            _lobbyRepository = lobbyRepository;
        }

        public async Task<int> CreateLobbyAsync(Lobby lobby, string username)
        {
            return await _lobbyRepository.CreateLobbyAsync(lobby.LobbyName, lobby.LobbyPassword, username);
        }
        public async Task DeleteLobbyAsync(int lobbyId)
        {
            await _lobbyRepository.DeleteLobbyAsync(lobbyId);
        }
        public async Task<Lobby> GetLobbyByNameAsync(string lobbyName)
        {
            return await _lobbyRepository.GetLobbyByNameAsync(lobbyName);
        }
        public async Task<Lobby> GetLobbyByIdAsync(int lobbyId)
        {
            return await _lobbyRepository.GetLobbyByIdAsync(lobbyId);
        }

        public async Task<IEnumerable<Lobby>> GetAllLobbiesAsync()
        {
            return await _lobbyRepository.GetAllLobbiesAsync();
        }
        public async Task UpdateLobbyStatusAsync(int lobbyId, string status)
        {
            await _lobbyRepository.UpdateLobbyStatusAsync(lobbyId, status);
        }

        public async Task UpdatePlayerCountAsync(int lobbyId, int playerCount)
        {
            await _lobbyRepository.UpdatePlayerCountAsync(lobbyId, playerCount);
        }
        public async Task AddPlayerToLobbyAsync(int lobbyId, string username)
        {
            await _lobbyRepository.AddPlayerToLobbyAsync(lobbyId, username);
        }
        public async Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username)
        {
            return await _lobbyRepository.IsPlayerInLobbyAsync(lobbyId, username);
        }
    }
}
