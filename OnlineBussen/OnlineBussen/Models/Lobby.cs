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
    }
}
