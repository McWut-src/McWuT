using McWuT.Data.Models;
using McWuT.Data.Repositories.Base;
using McWuT.Services.Shopping;
using System.Linq.Expressions;

namespace McWuT.Services
{
    public class ShoppingItemService(IUserEntityRepository<ShoppingItem> repository)
               : BaseUserEntityService<ShoppingItem>(repository), IShoppingItemService
    {
        public async Task<ShoppingItem> CreateAsync(string userId, int shoppingListId, string name, string? quantity = null, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            var shoppingItem = new ShoppingItem
            {
                UniqueId = Guid.NewGuid(),
                UserId = userId,
                ShoppingListId = shoppingListId,
                Name = name,
                Quantity = quantity,
                Category = category,
                IsPurchased = false
            };

            return await Repository.Create(shoppingItem);
        }

        public async Task<ShoppingItem> GetAsync(string userId, Guid uniqueId)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            var shoppingItem = await Repository.Get(uniqueId);
            
            if (shoppingItem == null || shoppingItem.UserId != userId)
                throw new KeyNotFoundException($"Shopping item with UniqueId {uniqueId} not found for user {userId}.");

            return shoppingItem;
        }

        public async Task<List<ShoppingItem>> GetByListAsync(string userId, int shoppingListId)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            Expression<Func<ShoppingItem, bool>> filter = item => item.ShoppingListId == shoppingListId;

            var result = await Repository.SearchAll(
                userId,
                filter: filter,
                orderBy: q => q.OrderBy(si => si.IsPurchased).ThenBy(si => si.Name)
            );

            return result.Items.ToList();
        }

        public async Task<ShoppingItem> UpdateAsync(string userId, Guid uniqueId, string name, string? quantity = null, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            var shoppingItem = await GetAsync(userId, uniqueId);
            shoppingItem.Name = name;
            shoppingItem.Quantity = quantity;
            shoppingItem.Category = category;

            return await Repository.Update(shoppingItem);
        }

        public async Task<ShoppingItem> TogglePurchasedAsync(string userId, Guid uniqueId)
        {
            var shoppingItem = await GetAsync(userId, uniqueId);
            shoppingItem.IsPurchased = !shoppingItem.IsPurchased;

            return await Repository.Update(shoppingItem);
        }

        public async Task DeleteAsync(string userId, Guid uniqueId)
        {
            var shoppingItem = await GetAsync(userId, uniqueId);
            await Repository.Delete(uniqueId);
        }
    }
}