using McWuT.Data.Models;
using McWuT.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class DeleteModel(INotesService notesService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string Id { get; set; }

    public Note Note { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var note = await notesService.Get(Id);
        if (note == null) return NotFound();
        Note = note;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var ok = await notesService.Delete(Id);
        if (ok == null) return NotFound();
        return RedirectToPage("Index");
    }
}