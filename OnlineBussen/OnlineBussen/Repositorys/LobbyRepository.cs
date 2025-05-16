using Microsoft.Data.SqlClient;
using OnlineBussen.Interfaces;
using OnlineBussen.Models;

namespace OnlineBussen.Repositorys
{
    public class LobbyRepository : ILobbyRepository
    {
        private readonly string _connectionString;

        public LobbyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateLobbyAsync(string lobbyName, string lobbyPassword, string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = @"INSERT INTO Lobbys (LobbyName, LobbyPassword, CreatedDate, Status, AmountOfPlayers, Host) 
                      VALUES (@LobbyName, @LobbyPassword, @CreatedDate, @Status, @AmountOfPlayers, @Host);
                      SELECT CAST(SCOPE_IDENTITY() as int)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyName", lobbyName);
                    command.Parameters.AddWithValue("@LobbyPassword", lobbyPassword);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Preparing game");
                    command.Parameters.AddWithValue("@AmountOfPlayers", 1);
                    command.Parameters.AddWithValue("@Host", username);

                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }
        public async Task DeleteLobbyAsync(int lobbyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "DELETE FROM Lobbys WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Lobby> GetLobbyByNameAsync(string lobbyName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Lobbys WHERE LobbyName = @LobbyName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyName", lobbyName);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Lobby
                            {
                                LobbyId = reader.GetInt32(0),
                                LobbyName = reader.GetString(1),
                                LobbyPassword = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IEnumerable<Lobby>> GetAllLobbiesAsync()
        {
            var lobbies = new List<Lobby>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Lobbys ORDER BY CreatedDate DESC";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lobbies.Add(new Lobby
                            {
                                LobbyId = reader.GetInt32(0),
                                LobbyName = reader.GetString(1),
                                LobbyPassword = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3),
                                Status = reader.GetString(4),
                                AmountOfPlayers = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return lobbies;
        }
        public async Task<Lobby> GetLobbyByIdAsync(int lobbyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT LobbyId, LobbyName, LobbyPassword, CreatedDate, Status, AmountOfPlayers, Host FROM Lobbys WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Lobby
                            {
                                LobbyId = reader.GetInt32(0),
                                LobbyName = reader.GetString(1),
                                LobbyPassword = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3),
                                Status = reader.GetString(4),
                                AmountOfPlayers = reader.GetInt32(5),
                                Host = reader.GetString(6)
                            };
                        }
                        return null;
                    }
                }
            }
        }
        public async Task UpdateLobbyStatusAsync(int lobbyId, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "UPDATE Lobbys SET Status = @Status WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task UpdatePlayerCountAsync(int lobbyId, int playerCount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "UPDATE Lobbys SET AmountOfPlayers = @AmountOfPlayers WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@AmountOfPlayers", playerCount);
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
