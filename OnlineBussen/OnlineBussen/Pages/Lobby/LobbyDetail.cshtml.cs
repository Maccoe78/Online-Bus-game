using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Controllers;
using OnlineBussen.Interfaces;
using OnlineBussen.Models;

namespace OnlineBussen.Pages.Lobby
{
    public class LobbyDetailModel : PageModel
    {
        private readonly LobbyController _lobbyController;

        public LobbyDetailModel(LobbyController lobbyController)
        {
            _lobbyController = lobbyController;
        }

        public Models.Lobby Lobby { get; set; }
        public List<User> Players { get; set; } = new List<User>();
        public bool IsHost { get; set; }

        public async Task<IActionResult> OnGetAsync(int lobbyId)
        {
            // Haal de huidige gebruikersnaam op uit de sessie
            string currentUsername = HttpContext.Session.GetString("Username");
            ViewData["Username"] = currentUsername;

            // Haal lobby informatie op
            Lobby = await _lobbyController.GetLobbyByIdAsync(lobbyId);

            if (Lobby == null)
            {
                return NotFound();
            }

            // Bepaal of de huidige gebruiker de host is
            IsHost = currentUsername == Lobby.Host;

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteLobbyAsync(int lobbyId)
        {

            // Controleer of de huidige gebruiker de host is
            //if (!IsHost)
            //{
            //    return Forbid();
            //}

            await _lobbyController.DeleteLobbyAsync(lobbyId);
            return RedirectToPage("/Lobby/Lobby");
        }

    }
}
