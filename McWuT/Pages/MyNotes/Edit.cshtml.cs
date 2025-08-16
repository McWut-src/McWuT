using McWuT.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class EditModel(INotesService notesService, UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string Id { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var note = await _notesService.Get(Id);
        if (note == null) return NotFound();

        Input = new InputModel { Title = note.Title, Content = note.Content };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var ok = await _notesService.Update(Id, Input.Title, Input.Content);
        if (ok == null) return NotFound();
        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [StringLength(4000)]
        public string? Content { get; set; }

        [StringLength(256)]
        public string? Title { get; set; }
    }

    private readonly INotesService _notesService = notesService;

    private readonly UserManager<IdentityUser> _userManager = userManager;
}