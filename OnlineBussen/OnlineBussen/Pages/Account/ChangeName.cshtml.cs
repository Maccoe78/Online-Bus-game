using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;
using OnlineBussen.Presentation.Models;

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
                user.Username = username;
                user.Password = password;

                var result = await _userController.UpdateUserAsync(user, username);
               
                if (!result.succes)
                {
                    ModelState.AddModelError("CurrentUser.Username", result.message);

                    CurrentUser = user;
                    ViewData["Username"] = currentUsername;
                    return Page();
                }
                
                // Update session
                HttpContext.Session.SetString("Username", username);

                return RedirectToPage("/Account/Account");
            }

            return Page();
        }
    }
}
