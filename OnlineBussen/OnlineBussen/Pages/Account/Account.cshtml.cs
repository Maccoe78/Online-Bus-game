using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using OnlineBussen.Logic.Models;
using OnlineBussen.Presentation.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using OnlineBussen.Logic.Controllers;


namespace OnlineBussen.Pages.Account
{
    [Authentication]
    public class AccountModel : PageModel
    {
        private readonly UserController _userController;
        public User CurrentUser { get; set; }

        public AccountModel(UserController usercontroller)
        {
            _userController = usercontroller;
           
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;

            CurrentUser = await _userController.GetUserByUsernameAsync(username);
            return Page();
        }
    }
}
