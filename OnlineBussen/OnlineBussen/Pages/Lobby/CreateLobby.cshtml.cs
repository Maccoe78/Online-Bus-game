using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using OnlineBussen.Controllers;
using OnlineBussen.Models;

namespace OnlineBussen.Pages.Lobby
{
    public class CreateLobbyModel : PageModel
    {
        private readonly LobbyController _lobbyController;

        public CreateLobbyModel(LobbyController lobbyController)
        {
            _lobbyController = lobbyController;
        }

        [BindProperty]
        public Models.Lobby Lobby { get; set; }

        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            string username = HttpContext.Session.GetString("Username");
            int lobbyId = await _lobbyController.CreateLobbyAsync(Lobby, username);

            return RedirectToPage("/Lobby/LobbyDetail", new { lobbyId = lobbyId });
        }
    }
}
