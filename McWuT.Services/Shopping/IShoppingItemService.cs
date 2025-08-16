using McWuT.Data.Models;

namespace McWuT.Services.Shopping
{
    public interface IShoppingItemService
    {
        Task<ShoppingItem> CreateAsync(string userId, int shoppingListId, string name, string? quantity = null, string? category = null);
        Task<ShoppingItem> GetAsync(string userId, Guid uniqueId);
        Task<List<ShoppingItem>> GetByListAsync(string userId, int shoppingListId);
        Task<ShoppingItem> UpdateAsync(string userId, Guid uniqueId, string name, string? quantity = null, string? category = null);
        Task<ShoppingItem> TogglePurchasedAsync(string userId, Guid uniqueId);
        Task DeleteAsync(string userId, Guid uniqueId);
    }
}