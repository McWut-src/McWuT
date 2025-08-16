using McWuT.Data.Models;
using McWuT.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.MyNotes;

[Authorize]
public class IndexModel(INotesService notesService, UserManager<IdentityUser> userManager) : PageModel
{
    public List<Note> Notes { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    [Display(Name = "Search")]
    public string? Query { get; set; }

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        Notes = await _notesService.GetNotes(userId, Query);
    }

    private readonly INotesService _notesService = notesService;

    private readonly UserManager<IdentityUser> _userManager = userManager;
}