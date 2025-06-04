using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using OnlineBussen.Presentation.Models;
using System.Numerics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;


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
        public Logic.Models.User User { get; set; }


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
