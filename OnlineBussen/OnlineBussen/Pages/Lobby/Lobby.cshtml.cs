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

        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;

            // Check if user is in a lobby
            var activeLobbyId = await _lobbyController.GetUserActiveLobbyIdAsync(username);
            if (activeLobbyId.HasValue)
            {
                // Redirect to LobbyDetail if user is in a lobby
                return RedirectToPage("/Lobby/LobbyDetail", new { lobbyId = activeLobbyId.Value });
            }

            // Otherwise show lobby list
            Lobbies = await _lobbyController.GetAllLobbiesAsync();
            return Page();
        }
    }
}
