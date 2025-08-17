using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.Tools;

[AllowAnonymous]
public class RandomUserModel : PageModel
{
    public void OnGet()
    {
        // Page is client-side interactive; API calls are made via JavaScript
    }
}