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
    public class IndexModel : PageModel
    {
        public const int PageSize = 15;

        public IndexModel(IPasswordVaultService passwordVaultService, UserManager<IdentityUser> userManager)
        {
            _passwordVaultService = passwordVaultService;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public List<PasswordEntry> Entries { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public int TotalPages { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public Guid? EditingId { get; set; }

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
                    .OrderByDescending(e => e.UpdatedAt ?? e.CreatedAt)
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

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            await _passwordVaultService.CreateEntryAsync(
                userId,
                Input.Name,
                Input.Website,
                Input.Username,
                Input.Password,
                Input.Notes,
                Input.Category
            );

            TempData["SuccessMessage"] = "Password entry created successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!EditingId.HasValue || !ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var success = await _passwordVaultService.UpdateEntryAsync(
                userId,
                EditingId.Value,
                Input.Name,
                Input.Website,
                Input.Username,
                Input.Password,
                Input.Notes,
                Input.Category
            );

            if (!success)
            {
                ModelState.AddModelError("", "Entry not found or you don't have permission to update it.");
                await OnGetAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Password entry updated successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var success = await _passwordVaultService.DeleteEntryAsync(userId, id);
            if (!success)
                return new JsonResult(new { success = false, message = "Entry not found or you don't have permission to delete it." });

            return new JsonResult(new { success = true, message = "Password entry deleted successfully!" });
        }

        public async Task<IActionResult> OnGetEditAsync(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var entry = await _passwordVaultService.GetEntryAsync(userId, id);
            if (entry == null)
                return NotFound();

            return new JsonResult(new
            {
                id = entry.UniqueId,
                name = entry.Name,
                website = entry.Website,
                username = entry.Username,
                category = entry.Category,
                notes = entry.Notes,
                hasPassword = !string.IsNullOrEmpty(entry.EncryptedPassword)
            });
        }

        public class InputModel
        {
            [Required]
            [MaxLength(256)]
            [Display(Name = "Entry Name")]
            public string Name { get; set; } = string.Empty;

            [MaxLength(256)]
            [Display(Name = "Website")]
            public string? Website { get; set; }

            [MaxLength(256)]
            [Display(Name = "Username")]
            public string? Username { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string? Password { get; set; }

            [MaxLength(100)]
            [Display(Name = "Category")]
            public string? Category { get; set; }

            [MaxLength(1000)]
            [Display(Name = "Notes")]
            [DataType(DataType.MultilineText)]
            public string? Notes { get; set; }
        }

        private readonly IPasswordVaultService _passwordVaultService;
        private readonly UserManager<IdentityUser> _userManager;
    }
}