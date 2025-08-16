using McWuT.Data.Models;
using McWuT.Data.Repositories.Base;
using McWuT.Services.Shopping;

namespace McWuT.Services
{
    public class ShoppingListService(IUserEntityRepository<ShoppingList> repository)
               : BaseUserEntityService<ShoppingList>(repository), IShoppingListService
    {
        public async Task<ShoppingList> CreateAsync(string userId, string name)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            var shoppingList = new ShoppingList
            {
                UniqueId = Guid.NewGuid(),
                UserId = userId,
                Name = name
            };

            return await Repository.Create(shoppingList);
        }

        public async Task<ShoppingList> GetAsync(string userId, Guid uniqueId)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            var shoppingList = await Repository.Get(uniqueId);
            
            if (shoppingList == null || shoppingList.UserId != userId)
                throw new KeyNotFoundException($"Shopping list with UniqueId {uniqueId} not found for user {userId}.");

            return shoppingList;
        }

        public async Task<List<ShoppingList>> GetAllAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            var result = await Repository.SearchAll(
                userId,
                orderBy: q => q.OrderByDescending(sl => sl.CreatedDate)
            );

            return result.Items.ToList();
        }

        public async Task<ShoppingList> UpdateAsync(string userId, Guid uniqueId, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            var shoppingList = await GetAsync(userId, uniqueId);
            shoppingList.Name = name;

            return await Repository.Update(shoppingList);
        }

        public async Task DeleteAsync(string userId, Guid uniqueId)
        {
            var shoppingList = await GetAsync(userId, uniqueId);
            await Repository.Delete(uniqueId);
        }
    }
}