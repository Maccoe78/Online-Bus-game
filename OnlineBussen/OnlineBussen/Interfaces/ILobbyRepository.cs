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
    }
}
