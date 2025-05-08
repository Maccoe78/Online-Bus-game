using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using OnlineBussen.Models;
using System.Numerics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace OnlineBussen.Pages.Signup
{
    public class SignupModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public SignupModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public User User { get; set; }
        

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                string sql = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", User.Username);
                    command.Parameters.AddWithValue("@Password", User.Password); 
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToPage("/Account/Account");
        }
        
    }
}
