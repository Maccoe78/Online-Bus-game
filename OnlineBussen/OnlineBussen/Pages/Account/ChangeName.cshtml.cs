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
        public User CurrentUser { get; set; }

        public ChangeNameModel(UserController userController)
        {
            _userController = userController;
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
                user.Username = username;
                user.Password = password;
                await _userController.UpdateUserAsync(user);

                // Update de session met de nieuwe username
                HttpContext.Session.SetString("Username", username);

                return RedirectToPage("/Account/Account");
            }

            return Page();
        }
    }
}
