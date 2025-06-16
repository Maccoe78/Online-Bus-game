using System.Text.Json;

namespace OnlineBussen.Logic.Models
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
        public string JoinedPlayers { get; set; } = string.Empty;

        public List<User> GetJoinedUsers()
        {
            if (string.IsNullOrEmpty(JoinedPlayers))
                return new List<User>();

            try
            {
                // Probeer eerst JSON deserialisatie (nieuwe format)
                if (JoinedPlayers.StartsWith("["))
                {
                    return JsonSerializer.Deserialize<List<User>>(JoinedPlayers) ?? new List<User>();
                }
                else
                {
                    // Oude format (comma separated usernames) - backwards compatibility
                    var usernames = JoinedPlayers.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    return usernames.Select(username => new User { Username = username.Trim() }).ToList();
                }
            }
            catch
            {
                return new List<User>();
            }
        }

        public void SetJoinedUsers(List<User> users)
        {
            JoinedPlayers = JsonSerializer.Serialize(users);
            AmountOfPlayers = users.Count;
        }

        public void AddUser(User user)
        {
            var users = GetJoinedUsers();
            if (!users.Any(u => u.Username == user.Username))
            {
                users.Add(user);
                SetJoinedUsers(users);
            }
        }

        public void RemoveUser(string username)
        {
            var users = GetJoinedUsers();
            users.RemoveAll(u => u.Username == username);
            SetJoinedUsers(users);
        }

        public bool HasUser(string username)
        {
            return GetJoinedUsers().Any(u => u.Username == username);
        }

        // Backwards compatibility - behoud je bestaande methodes
        public List<string> GetJoinedPlayersAsStrings()
        {
            return GetJoinedUsers().Select(u => u.Username).ToList();
        }

        //public List<User> GetJoinedPlayers()
        //{
        //    try
        //    {
        //        return JsonSerializer.Deserialize<List<User>>(JoinedPlayers) ?? new List<User>();
        //    }
        //    catch
        //    {
        //        return new List<User>();
        //    }
        //}

        //public void AddPlayer(string username)
        //{
        //    var players = GetJoinedPlayers();
        //    if (!players.Contains(username))
        //    {
        //        players.Add(username);
        //        JoinedPlayers = JsonSerializer.Serialize(players);
        //    }
        //}
    }
}
