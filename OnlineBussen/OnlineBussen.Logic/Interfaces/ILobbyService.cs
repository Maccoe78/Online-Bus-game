using OnlineBussen.Logic.Models;

namespace OnlineBussen.Logic.Interfaces
{
    public interface ILobbyService
    {
        Task<int> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username);
        Task DeleteLobbyAsync(int lobbyId);
        Task<Lobby> GetLobbyByNameAsync(string lobbyName);
        Task<IEnumerable<Lobby>> GetAllLobbiesAsync();
        Task UpdateLobbyStatusAsync(int lobbyId, string status);
        Task<Lobby> GetLobbyByIdAsync(int lobbyId);
        Task AddPlayerToLobbyAsync(int lobbyId, string username);
        Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username);
        Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername);
        Task RemovePlayerFromLobbyAsync(int lobbyId, string username);
        Task<bool> LobbyNameExistsAsync(string lobbyName);
        Task<int?> GetUserActiveLobbyIdAsync(string username);
    }
}
