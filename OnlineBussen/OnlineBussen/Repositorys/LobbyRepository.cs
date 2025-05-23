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
                string sql = @"INSERT INTO Lobbys (LobbyName, LobbyPassword, CreatedDate, Status, AmountOfPlayers, Host, JoinedPlayers) 
                      VALUES (@LobbyName, @LobbyPassword, @CreatedDate, @Status, @AmountOfPlayers, @Host, @JoinedPlayers);
                      SELECT CAST(SCOPE_IDENTITY() as int)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    var initialPlayers = new List<string> { username }; // Host is eerste speler
                    var joinedPlayersJson = System.Text.Json.JsonSerializer.Serialize(initialPlayers);

                    command.Parameters.AddWithValue("@LobbyName", lobbyName);
                    command.Parameters.AddWithValue("@LobbyPassword", lobbyPassword);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Preparing game");
                    command.Parameters.AddWithValue("@AmountOfPlayers", 1);
                    command.Parameters.AddWithValue("@Host", username);
                    command.Parameters.AddWithValue("@JoinedPlayers", joinedPlayersJson);

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
                                AmountOfPlayers = reader.GetInt32(5),
                                JoinedPlayers = reader.GetString(6)
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
                string sql = "SELECT LobbyId, LobbyName, LobbyPassword, CreatedDate, Status, AmountOfPlayers, Host, JoinedPlayers FROM Lobbys WHERE LobbyId = @LobbyId";

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
                                Host = reader.GetString(6),
                                JoinedPlayers = reader.GetString(7)
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
        public async Task AddPlayerToLobbyAsync(int lobbyId, string currentUsername)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var lobby = await GetLobbyByIdAsync(lobbyId);
                lobby.AddPlayer(currentUsername);

                string sql = @"UPDATE Lobbys 
                      SET JoinedPlayers = @JoinedPlayers,
                          AmountOfPlayers = (SELECT COUNT(*) FROM OPENJSON(@JoinedPlayers))
                      WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    command.Parameters.AddWithValue("@JoinedPlayers", lobby.JoinedPlayers);
                    command.Parameters.AddWithValue("@AmountOfPlayers", lobby.AmountOfPlayers);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<bool> IsPlayerInLobbyAsync(int lobbyId, string currentUsername)
        {
            var lobby = await GetLobbyByIdAsync(lobbyId);
            return lobby.GetJoinedPlayers().Contains(currentUsername);
        }
        public async Task UpdatePlayerUsernameInLobbiesAsync(string oldUsername, string newUsername)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Eerst alle lobbies ophalen waar de speler in zit
                string selectSql = "SELECT LobbyId, Host, JoinedPlayers FROM Lobbys WHERE JoinedPlayers LIKE @Pattern";

                using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Pattern", $"%{oldUsername}%");

                    using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
                    {
                        var lobbiesToUpdate = new List<(int LobbyId, string Host, string JoinedPlayers)>();

                        while (await reader.ReadAsync())
                        {
                            lobbiesToUpdate.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2)
                            ));
                        }

                        reader.Close();

                        foreach (var lobby in lobbiesToUpdate)
                        {
                            // Update Host als dat nodig is
                            if (lobby.Host == oldUsername)
                            {
                                string updateHostSql = "UPDATE Lobbys SET Host = @NewUsername WHERE LobbyId = @LobbyId";
                                using (SqlCommand updateHostCommand = new SqlCommand(updateHostSql, connection))
                                {
                                    updateHostCommand.Parameters.AddWithValue("@NewUsername", newUsername);
                                    updateHostCommand.Parameters.AddWithValue("@LobbyId", lobby.LobbyId);
                                    await updateHostCommand.ExecuteNonQueryAsync();
                                }
                            }

                            // Update JoinedPlayers
                            var players = System.Text.Json.JsonSerializer.Deserialize<List<string>>(lobby.JoinedPlayers);
                            int index = players.IndexOf(oldUsername);
                            if (index != -1)
                            {
                                players[index] = newUsername;
                                string updatedPlayersJson = System.Text.Json.JsonSerializer.Serialize(players);

                                string updatePlayersSql = "UPDATE Lobbys SET JoinedPlayers = @JoinedPlayers WHERE LobbyId = @LobbyId";
                                using (SqlCommand updatePlayersCommand = new SqlCommand(updatePlayersSql, connection))
                                {
                                    updatePlayersCommand.Parameters.AddWithValue("@JoinedPlayers", updatedPlayersJson);
                                    updatePlayersCommand.Parameters.AddWithValue("@LobbyId", lobby.LobbyId);
                                    await updatePlayersCommand.ExecuteNonQueryAsync();
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task RemovePlayerFromLobbyAsync(int lobbyId, string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var lobby = await GetLobbyByIdAsync(lobbyId);
                var players = lobby.GetJoinedPlayers();
                players.Remove(username);

                string updatedPlayersJson = System.Text.Json.JsonSerializer.Serialize(players);

                string sql = @"UPDATE Lobbys 
                      SET JoinedPlayers = @JoinedPlayers,
                          AmountOfPlayers = (SELECT COUNT(*) FROM OPENJSON(@JoinedPlayers))
                      WHERE LobbyId = @LobbyId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyId", lobbyId);
                    command.Parameters.AddWithValue("@JoinedPlayers", updatedPlayersJson);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<bool> LobbyNameExistsAsync(string lobbyName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Lobbys WHERE LobbyName = @LobbyName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LobbyName", lobbyName);
                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

    }
}
