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
        public string JoinedPlayers { get; set; } = string.Empty;

        public List<UserDTO> GetJoinedUsers()
        {
            if (string.IsNullOrEmpty(JoinedPlayers))
                return new List<UserDTO>();

            try
            {
                // Probeer JSON deserialisatie (nieuwe format)
                if (JoinedPlayers.StartsWith("["))
                {
                    return JsonSerializer.Deserialize<List<UserDTO>>(JoinedPlayers) ?? new List<UserDTO>();
                }
                else
                {
                    // Oude format (comma separated) - backwards compatibility
                    var usernames = JoinedPlayers.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    return usernames.Select(username => new UserDTO { Username = username.Trim() }).ToList();
                }
            }
            catch
            {
                return new List<UserDTO>();
            }
        }
        public List<string> GetJoinedPlayers()
        {
            return GetJoinedUsers().Select(u => u.Username).ToList();
        }

        
    }
}
