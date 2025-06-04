using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;
using OnlineBussen.Presentation.Models;

namespace OnlineBussen.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserController _userController;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public IndexModel(UserController userController)
        {
            _userController = userController;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userController.AuthenticateUserAsync(Username, Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", Username);
                return RedirectToPage("/Account/Account");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear(); // Wist alle sessie data
            return RedirectToPage("/Index");
        }
    }
}
