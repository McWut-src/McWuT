using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class CreateModel : PageModel
{
    private readonly INotesService _notesService;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateModel(INotesService notesService, UserManager<IdentityUser> userManager)
    {
        _notesService = notesService;
        _userManager = userManager;
    }

    public class InputModel
    {
        [StringLength(256)]
        public string? Title { get; set; }

        [StringLength(4000)]
        public string? Content { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = _userManager.GetUserId(User)!;
        await _notesService.CreateAsync(userId, Input.Title, Input.Content);
        return RedirectToPage("Index");
    }
}
