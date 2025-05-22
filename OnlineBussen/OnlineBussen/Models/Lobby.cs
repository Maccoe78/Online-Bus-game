using System.Text.Json;

namespace OnlineBussen.Models
{
    public class Lobby
    {
        public int LobbyId { get; set; }
        public string LobbyName { get; set; }
        public string LobbyPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Host { get; set; }
        public string Status { get; set; }
        public int AmountOfPlayers { get; set; }
        private string _joinedPlayers { get; set; } = "[]";
        public string JoinedPlayers
        {
            get => _joinedPlayers;
            set => _joinedPlayers = string.IsNullOrEmpty(value) ? "[]" : value;
        }

        public List<string> GetJoinedPlayers()
        {
            try
            {
                return JsonSerializer.Deserialize<List<string>>(JoinedPlayers) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public void AddPlayer(string username)
        {
            var players = GetJoinedPlayers();
            if (!players.Contains(username))
            {
                players.Add(username);
                JoinedPlayers = JsonSerializer.Serialize(players);
            }
        }
    }
}
