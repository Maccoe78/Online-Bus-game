using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineBussen.Models;

namespace OnlineBussen.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", Password);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ViewData["Username"] = Username;
                            HttpContext.Session.SetString("Username", Username);
                            return RedirectToPage("/Account/Account");
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return Page();
        }
    }
}
