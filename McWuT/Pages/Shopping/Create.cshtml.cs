using McWuT.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.Shopping;

[Authorize]
public class CreateModel(IShoppingListService shoppingListService, UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty]
    [Required]
    [Display(Name = "List Name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var userId = _userManager.GetUserId(User)!;
            await _shoppingListService.CreateAsync(userId, Name);
            
            TempData["SuccessMessage"] = "Shopping list created successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while creating the shopping list.";
            return Page();
        }
    }

    private readonly IShoppingListService _shoppingListService = shoppingListService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
}