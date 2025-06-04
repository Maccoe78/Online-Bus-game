using OnlineBussen.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBussen.Data.Interfaces;
using OnlineBussen.Data.Models;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Logic.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly ILobbyRepository _lobbyRepository;

        public LobbyService(ILobbyRepository lobbyRepository)
        {
            _lobbyRepository = lobbyRepository;
        }

        public async Task<int> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username)
        {
            return await _lobbyRepository.CreateLobbyAsync(lobbyName, lobbyPassword, username);
        }
        public async Task DeleteLobbyAsync(int lobbyId)
        {
            await _lobbyRepository.DeleteLobbyAsync(lobbyId);
        }
        public async Task<Lobby> GetLobbyByNameAsync(string lobbyName)
        {
            var lobbyDto = await _lobbyRepository.GetLobbyByNameAsync(lobbyName);
            return new Lobby
            {
                LobbyId = lobbyDto.LobbyId,
                LobbyName = lobbyDto.LobbyName,
                LobbyPassword = lobbyDto.LobbyPassword,
                CreatedDate = lobbyDto.CreatedDate,
                Status = lobbyDto.Status,
                AmountOfPlayers = lobbyDto.AmountOfPlayers,
                Host = lobbyDto.Host,
                JoinedPlayers = lobbyDto.JoinedPlayers
            };
        }

        public async Task<IEnumerable<Lobby>> GetAllLobbiesAsync()
        {
            var lobbyDtos = await _lobbyRepository.GetAllLobbiesAsync();
            return lobbyDtos.Select(dto => new Lobby
            {
                LobbyId = dto.LobbyId,
                LobbyName = dto.LobbyName,
                LobbyPassword = dto.LobbyPassword,
                CreatedDate = dto.CreatedDate,
                Status = dto.Status,
                AmountOfPlayers = dto.AmountOfPlayers,
                Host = dto.Host,
                JoinedPlayers = dto.JoinedPlayers
            });
        }
        public async Task UpdateLobbyStatusAsync(int lobbyId, string status)
        {
            await _lobbyRepository.UpdateLobbyStatusAsync(lobbyId, status);
        }

        public async Task<Lobby> GetLobbyByIdAsync(int lobbyId)
        {
            var lobbyDto = await _lobbyRepository.GetLobbyByIdAsync(lobbyId);
            return new Lobby
            {
                LobbyId = lobbyDto.LobbyId,
                LobbyName = lobbyDto.LobbyName,
                LobbyPassword = lobbyDto.LobbyPassword,
                CreatedDate = lobbyDto.CreatedDate,
                Status = lobbyDto.Status,
                AmountOfPlayers = lobbyDto.AmountOfPlayers,
                Host = lobbyDto.Host,
                JoinedPlayers = lobbyDto.JoinedPlayers
            };
        }

        public async Task AddPlayerToLobbyAsync(int lobbyId, string username)
        {
            await _lobbyRepository.AddPlayerToLobbyAsync(lobbyId, username);
        }
        public async Task<bool> IsPlayerInLobbyAsync(int lobbyId, string username)
        {
            return await _lobbyRepository.IsPlayerInLobbyAsync(lobbyId, username);
        }

        public async Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername)
        {
            await _lobbyRepository.UpdatePlayerUsernameInLobbiesAsync(oldUsername, newUsername);
        }

        public async Task RemovePlayerFromLobbyAsync(int lobbyId, string username)
        {
            await _lobbyRepository.RemovePlayerFromLobbyAsync(lobbyId, username);
        }

        public async Task<bool> LobbyNameExistsAsync(string lobbyName)
        {
            return await _lobbyRepository.LobbyNameExistsAsync(lobbyName);
        }

        public async Task<int?> GetUserActiveLobbyIdAsync(string username)
        {
            return await _lobbyRepository.GetUserActiveLobbyIdAsync(username);
        }
    }
}
