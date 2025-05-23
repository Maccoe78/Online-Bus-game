using OnlineBussen.Models;

namespace OnlineBussen.Interfaces
{
    public interface ILobbyRepository
    {
        Task<int> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username);
        Task DeleteLobbyAsync(int lobbyId);
        Task<Models.Lobby> GetLobbyByNameAsync(string lobbyName);
        Task<IEnumerable<Models.Lobby>> GetAllLobbiesAsync();
        Task UpdateLobbyStatusAsync(int lobbyId, string status);
        Task UpdatePlayerCountAsync(int lobbyId, int playerCount);
        Task<Lobby> GetLobbyByIdAsync(int lobbyId);
        Task AddPlayerToLobbyAsync(int lobbyId, string username);
        Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username);
        Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername);
        Task RemovePlayerFromLobbyAsync(int lobbyId, string username);
        Task<bool> LobbyNameExistsAsync(string lobbyName);
        Task<int?> GetUserActiveLobbyIdAsync(string username);
    }
}
