using OnlineBussen.Data.Models;

namespace OnlineBussen.Data.Interfaces
{
    public interface ILobbyRepository
    {
        Task<int> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username);
        Task DeleteLobbyAsync(int lobbyId);
        Task<Models.LobbyDTO> GetLobbyByNameAsync(string lobbyName);
        Task<IEnumerable<Models.LobbyDTO>> GetAllLobbiesAsync();
        Task UpdateLobbyStatusAsync(int lobbyId, string status);
        Task UpdatePlayerCountAsync(int lobbyId, int playerCount);
        Task<LobbyDTO> GetLobbyByIdAsync(int lobbyId);
        Task AddPlayerToLobbyAsync(int lobbyId, string username);
        Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username);
        Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername);
        Task RemovePlayerFromLobbyAsync(int lobbyId, string username);
        Task<bool> LobbyNameExistsAsync(string lobbyName);
        Task<int?> GetUserActiveLobbyIdAsync(string username);
    }
}
