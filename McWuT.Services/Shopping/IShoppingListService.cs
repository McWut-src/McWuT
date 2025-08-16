using McWuT.Data.Models;

namespace McWuT.Services.Shopping
{
    public interface IShoppingListService
    {
        Task<ShoppingList> CreateAsync(string userId, string name);
        Task<ShoppingList> GetAsync(string userId, Guid uniqueId);
        Task<List<ShoppingList>> GetAllAsync(string userId);
        Task<ShoppingList> UpdateAsync(string userId, Guid uniqueId, string name);
        Task DeleteAsync(string userId, Guid uniqueId);
    }
}