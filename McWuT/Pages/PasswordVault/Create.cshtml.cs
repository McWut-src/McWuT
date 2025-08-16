using McWuT.Services.PasswordVault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.PasswordVault
{
    [Authorize]
    public class CreateModel : PageModel
    {
        public CreateModel(IPasswordVaultService passwordVaultService, UserManager<IdentityUser> userManager)
        {
            _passwordVaultService = passwordVaultService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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
            return RedirectToPage("./Index");
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
            [Display(Name = "Password")]
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