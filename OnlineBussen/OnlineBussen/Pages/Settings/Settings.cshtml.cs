using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBussen.Presentation.Models;

namespace OnlineBussen.Pages.Settings
{
    [Authentication]
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
        }
    }
}
