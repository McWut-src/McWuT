using McWuT.Data.Models;
using McWuT.Services.PasswordVault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.PasswordVault
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        public DeleteModel(IPasswordVaultService passwordVaultService, UserManager<IdentityUser> userManager)
        {
            _passwordVaultService = passwordVaultService;
            _userManager = userManager;
        }

        public PasswordEntry? Entry { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            Entry = await _passwordVaultService.GetEntryAsync(userId, id);
            if (Entry == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var success = await _passwordVaultService.DeleteEntryAsync(userId, id);
            if (!success)
                return NotFound();

            TempData["SuccessMessage"] = "Password entry deleted successfully!";
            return RedirectToPage("./Index");
        }

        private readonly IPasswordVaultService _passwordVaultService;

        private readonly UserManager<IdentityUser> _userManager;
    }
}