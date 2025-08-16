using McWuT.Data.Models;
using McWuT.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.Shopping;

[Authorize]
public class DeleteModel(IShoppingListService shoppingListService, IShoppingItemService shoppingItemService, UserManager<IdentityUser> userManager) : PageModel
{
    public ShoppingList ShoppingList { get; set; } = null!;
    public List<ShoppingItem> Items { get; set; } = [];

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
            Items = await _shoppingItemService.GetByListAsync(userId, ShoppingList.Id);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var userId = _userManager.GetUserId(User)!;
            await _shoppingListService.DeleteAsync(userId, id.Value);
            
            TempData["SuccessMessage"] = "Shopping list deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while deleting the shopping list.";
            return RedirectToPage("./Details", new { id });
        }
    }

    private readonly IShoppingListService _shoppingListService = shoppingListService;
    private readonly IShoppingItemService _shoppingItemService = shoppingItemService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
}