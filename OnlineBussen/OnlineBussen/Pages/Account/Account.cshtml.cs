using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using OnlineBussen.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;


namespace OnlineBussen.Pages.Account
{
    [Authentication]
    public class AccountModel : PageModel
    {
        private readonly IConfiguration _configuration;
       public User CurrentUser { get; set; }

        public AccountModel(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Index");
            }

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            CurrentUser = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return Page();
        }
    }
}
