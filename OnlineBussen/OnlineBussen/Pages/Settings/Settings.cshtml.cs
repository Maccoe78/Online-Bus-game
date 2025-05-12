using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineBussen.Pages.Settings
{
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
        }
    }
}
