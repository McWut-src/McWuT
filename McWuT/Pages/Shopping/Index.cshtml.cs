using McWuT.Data.Models;
using McWuT.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace McWuT.Web.Pages.Shopping;

[Authorize]
public class IndexModel(IShoppingListService shoppingListService, IShoppingItemService shoppingItemService, UserManager<IdentityUser> userManager) : PageModel
{
    public List<ShoppingList> ShoppingLists { get; set; } = [];
    public Dictionary<int, int> ItemCounts { get; set; } = new();

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        ShoppingLists = await _shoppingListService.GetAllAsync(userId);
        
        // Get item counts for each list
        foreach (var list in ShoppingLists)
        {
            var items = await _shoppingItemService.GetByListAsync(userId, list.Id);
            ItemCounts[list.Id] = items.Count;
        }
    }

    private readonly IShoppingListService _shoppingListService = shoppingListService;
    private readonly IShoppingItemService _shoppingItemService = shoppingItemService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
}