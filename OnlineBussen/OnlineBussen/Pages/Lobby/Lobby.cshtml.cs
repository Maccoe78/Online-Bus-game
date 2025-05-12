using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineBussen.Pages.Lobby
{
    public class LobbyModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
        }
    }
}
