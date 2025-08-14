using McWuT.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly INotesService _notesService;
    private readonly UserManager<IdentityUser> _userManager;

    public DeleteModel(INotesService notesService, UserManager<IdentityUser> userManager)
    { _notesService = notesService; _userManager = userManager; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Note Note { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        var note = await _notesService.GetByIdAsync(userId, Id);
        if (note == null) return NotFound();
        Note = note;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        var ok = await _notesService.SoftDeleteAsync(userId, Id);
        if (!ok) return NotFound();
        return RedirectToPage("Index");
    }
}
