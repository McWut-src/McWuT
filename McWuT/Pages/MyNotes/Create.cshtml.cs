using McWuT.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class CreateModel : PageModel
{
    public CreateModel(INotesService notesService, UserManager<IdentityUser> userManager)
    {
        _notesService = notesService;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet()
    { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = _userManager.GetUserId(User)!;
        await _notesService.Create(userId, Input.Title, Input.Content);
        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [StringLength(4000)]
        public string? Content { get; set; }

        [StringLength(256)]
        public string? Title { get; set; }
    }

    private readonly INotesService _notesService;

    private readonly UserManager<IdentityUser> _userManager;
}