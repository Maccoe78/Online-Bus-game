using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using OnlineBussen.Models;
using System.Numerics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineBussen.Controllers;


namespace OnlineBussen.Pages.Signup
{
    public class SignupModel : PageModel
    {
        private readonly UserController _userController;

        public SignupModel(UserController userContoller)
        {
            _userController = userContoller;
        }

        [BindProperty]
        public Models.User User { get; set; }


        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _userController.CreateUserAsync(User.Username, User.Password);

            if (!result.success)
            {
                ModelState.AddModelError("User.Username", result.message);
                return Page();
            }

            return RedirectToPage("/Account/Account");
        }

    }
}
