using McWuT.Data.Models;
using McWuT.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class DetailsModel(INotesService notesService) : PageModel
{
    public Note Note { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var note = await _notesService.Get(id);
        if (note == null) return NotFound();
        Note = note;
        return Page();
    }

    private readonly INotesService _notesService = notesService;
}