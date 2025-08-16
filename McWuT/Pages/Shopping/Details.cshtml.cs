using McWuT.Data.Models;
using McWuT.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Web.Pages.Shopping;

[Authorize]
public class DetailsModel(IShoppingListService shoppingListService, IShoppingItemService shoppingItemService, UserManager<IdentityUser> userManager) : PageModel
{
    public ShoppingList ShoppingList { get; set; } = null!;
    public List<ShoppingItem> Items { get; set; } = [];

    [BindProperty]
    [Required]
    [Display(Name = "Item Name")]
    [MaxLength(256)]
    public string ItemName { get; set; } = string.Empty;

    [BindProperty]
    [Display(Name = "Quantity")]
    [MaxLength(50)]
    public string? ItemQuantity { get; set; }

    [BindProperty]
    [Display(Name = "Category")]
    [MaxLength(100)]
    public string? ItemCategory { get; set; }

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

    public async Task<IActionResult> OnPostAddItemAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User)!;
            ShoppingList = await _shoppingListService.GetAsync(userId, id.Value);
            Items = await _shoppingItemService.GetByListAsync(userId, ShoppingList.Id);
            return Page();
        }

        try
        {
            var userId = _userManager.GetUserId(User)!;
            ShoppingList = await _shoppingListService.GetAsync(userId, id.Value);
            await _shoppingItemService.CreateAsync(userId, ShoppingList.Id, ItemName, ItemQuantity, ItemCategory);
            
            TempData["SuccessMessage"] = "Item added successfully!";
            return RedirectToPage("./Details", new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while adding the item.";
            var userId = _userManager.GetUserId(User)!;
            ShoppingList = await _shoppingListService.GetAsync(userId, id.Value);
            Items = await _shoppingItemService.GetByListAsync(userId, ShoppingList.Id);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostTogglePurchasedAsync(Guid itemId)
    {
        try
        {
            var userId = _userManager.GetUserId(User)!;
            await _shoppingItemService.TogglePurchasedAsync(userId, itemId);
            return new JsonResult(new { success = true });
        }
        catch (Exception)
        {
            return new JsonResult(new { success = false });
        }
    }

    public async Task<IActionResult> OnPostDeleteItemAsync(Guid itemId, Guid? id)
    {
        try
        {
            var userId = _userManager.GetUserId(User)!;
            await _shoppingItemService.DeleteAsync(userId, itemId);
            TempData["SuccessMessage"] = "Item deleted successfully!";
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while deleting the item.";
        }

        return RedirectToPage("./Details", new { id });
    }

    private readonly IShoppingListService _shoppingListService = shoppingListService;
    private readonly IShoppingItemService _shoppingItemService = shoppingItemService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
}