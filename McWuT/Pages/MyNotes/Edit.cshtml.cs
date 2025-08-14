using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class EditModel : PageModel
{
    private readonly INotesService _notesService;
    private readonly UserManager<IdentityUser> _userManager;

    public EditModel(INotesService notesService, UserManager<IdentityUser> userManager)
    {
        _notesService = notesService; _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public class InputModel
    {
        [StringLength(256)]
        public string? Title { get; set; }
        [StringLength(4000)]
        public string? Content { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        var note = await _notesService.GetByIdAsync(userId, Id);
        if (note == null) return NotFound();

        Input = new InputModel { Title = note.Title, Content = note.Content };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var userId = _userManager.GetUserId(User)!;
        var ok = await _notesService.UpdateAsync(userId, Id, Input.Title, Input.Content);
        if (!ok) return NotFound();
        return RedirectToPage("Index");
    }
}
