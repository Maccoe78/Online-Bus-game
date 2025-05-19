using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Controllers;
using OnlineBussen.Models;

namespace OnlineBussen.Pages.Lobby
{
    [Authentication]
    public class LobbyModel : PageModel
    {
        private readonly LobbyController _lobbyController;

        public LobbyModel(LobbyController lobbyController)
        {
            _lobbyController = lobbyController;
        }

        public IEnumerable<Models.Lobby> Lobbies { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            Lobbies = await _lobbyController.GetAllLobbiesAsync();
        }
    }
}
