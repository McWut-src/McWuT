using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;
using McWuT.Data.Models;

namespace McWuT.Web.Pages.PasswordVault
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IPasswordVaultService _passwordVaultService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IPasswordVaultService passwordVaultService, UserManager<IdentityUser> userManager)
        {
            _passwordVaultService = passwordVaultService;
            _userManager = userManager;
        }

        public List<PasswordEntry> Entries { get; set; } = new();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }
        public const int PageSize = 10;

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var allEntries = await _passwordVaultService.GetEntriesAsync(userId, SearchTerm);
                
                // Calculate pagination
                TotalPages = (int)Math.Ceiling(allEntries.Count / (double)PageSize);
                
                // Get entries for current page
                Entries = allEntries
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPostRevealPasswordAsync(Guid entryId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var entry = await _passwordVaultService.GetEntryAsync(userId, entryId);
            if (entry == null)
                return NotFound();

            if (string.IsNullOrEmpty(entry.EncryptedPassword))
                return new JsonResult(new { password = "" });

            var decryptedPassword = _passwordVaultService.DecryptPassword(entry.EncryptedPassword);
            return new JsonResult(new { password = decryptedPassword });
        }
    }
}