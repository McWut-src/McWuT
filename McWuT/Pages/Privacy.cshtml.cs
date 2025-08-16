using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages
{
    public class PrivacyModel : PageModel
    {
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        private readonly ILogger<PrivacyModel> _logger;
    }
}