using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Controllers;
using OnlineBussen.Models;

namespace OnlineBussen.Pages.Account
{
    [Authentication]
    public class ChangeNameModel : PageModel
    {
        private readonly UserController _userController;
        private readonly LobbyController _lobbyController;
        public User CurrentUser { get; set; }

        public ChangeNameModel(UserController userController, LobbyController lobbyController)
        {
            _userController = userController;
            _lobbyController = lobbyController;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;

            CurrentUser = await _userController.GetUserByUsernameAsync(username);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var currentUsername = HttpContext.Session.GetString("Username");
            var user = await _userController.GetUserByUsernameAsync(currentUsername);

            if (user != null)
            {
                await _lobbyController.UpdatePlayerUsernameInLobbiesAsync(currentUsername, username);

                // Then update the user
                user.Username = username;
                user.Password = password;
                await _userController.UpdateUserAsync(user);

                // Update session
                HttpContext.Session.SetString("Username", username);

                return RedirectToPage("/Account/Account");
            }

            return Page();
        }
    }
}
