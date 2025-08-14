using McWuT.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly INotesService _notesService;
    private readonly UserManager<IdentityUser> _userManager;

    public DetailsModel(INotesService notesService, UserManager<IdentityUser> userManager)
    { _notesService = notesService; _userManager = userManager; }

    public Note Note { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = _userManager.GetUserId(User)!;
        var note = await _notesService.GetByIdAsync(userId, id);
        if (note == null) return NotFound();
        Note = note;
        return Page();
    }
}
