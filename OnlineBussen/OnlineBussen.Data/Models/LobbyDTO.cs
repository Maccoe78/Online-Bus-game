using System.Text.Json;

namespace OnlineBussen.Data.Models
{
    public class LobbyDTO
    {
        public int LobbyId { get; set; }
        public string LobbyName { get; set; }
        public string LobbyPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Host { get; set; }
        public string Status { get; set; }
        public int AmountOfPlayers { get; set; }
        public string JoinedPlayers { get; set; } = "[]";

        public List<string> GetJoinedPlayers()
        {
            return System.Text.Json.JsonSerializer.Deserialize<List<string>>(JoinedPlayers);
        }

        public void AddPlayer(string username)
        {
            var players = GetJoinedPlayers();
            if (!players.Contains(username))
            {
                players.Add(username);
                JoinedPlayers = System.Text.Json.JsonSerializer.Serialize(players);
            }
        }
    }
}
