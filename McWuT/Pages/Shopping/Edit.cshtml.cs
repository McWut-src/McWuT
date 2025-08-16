using McWuT.Data.Models;
using McWuT.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.Shopping;

[Authorize]
public class EditModel(IShoppingListService shoppingListService, UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty]
    public ShoppingList ShoppingList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var userId = _userManager.GetUserId(User)!;
            ShoppingList = await _shoppingListService.GetAsync(userId, id.Value);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
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
            await _shoppingListService.UpdateAsync(userId, ShoppingList.UniqueId, ShoppingList.Name);
            
            TempData["SuccessMessage"] = "Shopping list updated successfully!";
            return RedirectToPage("./Details", new { id = ShoppingList.UniqueId });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while updating the shopping list.";
            return Page();
        }
    }

    private readonly IShoppingListService _shoppingListService = shoppingListService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
}