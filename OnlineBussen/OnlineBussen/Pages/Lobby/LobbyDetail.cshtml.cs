using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Models;
using OnlineBussen.Presentation.Models;

namespace OnlineBussen.Pages.Lobby
{
    [Authentication]
    public class LobbyDetailModel : PageModel
    {
        private readonly LobbyController _lobbyController;

        public LobbyDetailModel(LobbyController lobbyController)
        {
            _lobbyController = lobbyController;
        }

        public Logic.Models.Lobby Lobby { get; set; }
        public List<User> Players { get; set; } = new List<User>();
        public bool IsHost { get; set; }
        public bool HasJoined { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(int lobbyId)
        {
            string currentUsername = HttpContext.Session.GetString("Username");
            ViewData["Username"] = currentUsername;

            Lobby = await _lobbyController.GetLobbyByIdAsync(lobbyId);

            if (Lobby == null)
            {
                return NotFound();
            }

            IsHost = currentUsername == Lobby.Host;

            HasJoined = Lobby.GetJoinedPlayers().Contains(currentUsername);

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteLobbyAsync(int lobbyId)
        {

            await _lobbyController.DeleteLobbyAsync(lobbyId);
            return RedirectToPage("/Lobby/Lobby");
        }
        public async Task<IActionResult> OnPostJoinLobbyAsync(int lobbyId, string password)
        {
            var lobby = await _lobbyController.GetLobbyByIdAsync(lobbyId);
            string currentUsername = HttpContext.Session.GetString("Username");

            if (lobby == null || lobby.LobbyPassword != password)
            {
                ModelState.AddModelError("Password", "Password is not correct");

                Lobby = lobby ?? await _lobbyController.GetLobbyByIdAsync(lobbyId);

                
                ViewData["Username"] = currentUsername;
                IsHost = currentUsername == Lobby.Host;
                HasJoined = Lobby.GetJoinedPlayers().Contains(currentUsername);

                return Page();
            }

            
            await _lobbyController.AddPlayerToLobbyAsync(lobbyId, currentUsername);

            // Refresh the lobby data after joining
            Lobby = await _lobbyController.GetLobbyByIdAsync(lobbyId);

            return RedirectToPage("/Lobby/LobbyDetail", new { lobbyId });
        }
        public async Task<IActionResult> OnPostLeaveLobbyAsync(int lobbyId)
        {
            string currentUsername = HttpContext.Session.GetString("Username");
            await _lobbyController.RemovePlayerFromLobbyAsync(lobbyId, currentUsername);
            return RedirectToPage("/Lobby/Lobby");
        }

    }
}
