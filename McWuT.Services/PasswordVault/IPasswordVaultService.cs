using McWuT.Data.Models;

namespace McWuT.Services.PasswordVault
{
    public interface IPasswordVaultService
    {
        Task<Guid> CreateEntryAsync(string userId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null);

        string DecryptPassword(string encryptedPassword);

        Task<bool> DeleteEntryAsync(string userId, Guid uniqueId);

        string EncryptPassword(string plainTextPassword);

        Task<List<PasswordEntry>> GetEntriesAsync(string userId, string? search = null);

        Task<PasswordEntry?> GetEntryAsync(string userId, Guid uniqueId);

        Task<bool> UpdateEntryAsync(string userId, Guid uniqueId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null);
    }
}