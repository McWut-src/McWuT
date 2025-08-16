using McWuT.Data.Models;
using McWuT.Services.PasswordVault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.PasswordVault
{
    [Authorize]
    public class EditModel : PageModel
    {
        public EditModel(IPasswordVaultService passwordVaultService, UserManager<IdentityUser> userManager)
        {
            _passwordVaultService = passwordVaultService;
            _userManager = userManager;
        }

        public PasswordEntry? Entry { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            Entry = await _passwordVaultService.GetEntryAsync(userId, id);
            if (Entry == null)
                return NotFound();

            Input.Name = Entry.Name;
            Input.Website = Entry.Website;
            Input.Username = Entry.Username;
            Input.Notes = Entry.Notes;
            Input.Category = Entry.Category;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            Entry = await _passwordVaultService.GetEntryAsync(userId, id);
            if (Entry == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var success = await _passwordVaultService.UpdateEntryAsync(
                userId,
                id,
                Input.Name,
                Input.Website,
                Input.Username,
                Input.Password, // Only update password if provided
                Input.Notes,
                Input.Category
            );

            if (!success)
                return NotFound();

            TempData["SuccessMessage"] = "Password entry updated successfully!";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRevealCurrentPasswordAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var entry = await _passwordVaultService.GetEntryAsync(userId, id);
            if (entry == null)
                return NotFound();

            if (string.IsNullOrEmpty(entry.EncryptedPassword))
                return new JsonResult(new { password = "" });

            var decryptedPassword = _passwordVaultService.DecryptPassword(entry.EncryptedPassword);
            return new JsonResult(new { password = decryptedPassword });
        }

        public class InputModel
        {
            [MaxLength(100)]
            [Display(Name = "Category")]
            public string? Category { get; set; }

            [Required]
            [MaxLength(256)]
            [Display(Name = "Entry Name")]
            public string Name { get; set; } = string.Empty;

            [MaxLength(1000)]
            [Display(Name = "Notes")]
            [DataType(DataType.MultilineText)]
            public string? Notes { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string? Password { get; set; }

            [MaxLength(256)]
            [Display(Name = "Username")]
            public string? Username { get; set; }

            [MaxLength(256)]
            [Display(Name = "Website")]
            public string? Website { get; set; }
        }

        private readonly IPasswordVaultService _passwordVaultService;

        private readonly UserManager<IdentityUser> _userManager;
    }
}