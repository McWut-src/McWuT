using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.Tools;

[AllowAnonymous]
public class JsonBeautifierModel : PageModel
{
    public void OnGet()
    {
        // Page is client-side interactive; no server actions needed.
    }
}
