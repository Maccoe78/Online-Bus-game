using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;
using OnlineBussen.Presentation.Models;

namespace OnlineBussen.Pages.Lobby
{
    [Authentication]
    public class CreateLobbyModel : PageModel
    {
        private readonly LobbyController _lobbyController;

        public CreateLobbyModel(LobbyController lobbyController)
        {
            _lobbyController = lobbyController;
        }

        [BindProperty]
        public Logic.Models.Lobby Lobby { get; set; }

        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string username = HttpContext.Session.GetString("Username");
            var result = await _lobbyController.CreateLobbyAsync(Lobby.LobbyName, Lobby.LobbyPassword, username);

            if (!result.success)
            {
                ModelState.AddModelError("Lobby.LobbyName", result.message);
                return Page();
            }

            return RedirectToPage("/Lobby/Lobby");
        }
    }
}
