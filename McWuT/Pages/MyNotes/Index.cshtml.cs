using System.ComponentModel.DataAnnotations;
using McWuT.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using McWuT.Data.Services;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class IndexModel : PageModel
{
    private readonly INotesService _notesService;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(INotesService notesService, UserManager<IdentityUser> userManager)
    {
        _notesService = notesService;
        _userManager = userManager;
    }

    public List<Note> Notes { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    [Display(Name = "Search")]
    public string? Query { get; set; }

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        Notes = await _notesService.GetNotesAsync(userId, Query);
    }
}
