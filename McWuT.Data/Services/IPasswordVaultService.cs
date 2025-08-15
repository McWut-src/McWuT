using McWuT.Data.Models;

namespace McWuT.Data.Services
{
    public interface IPasswordVaultService
    {
        Task<List<PasswordEntry>> GetEntriesAsync(string userId, string? search = null);
        Task<PasswordEntry?> GetEntryAsync(string userId, Guid uniqueId);
        Task<Guid> CreateEntryAsync(string userId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null);
        Task<bool> UpdateEntryAsync(string userId, Guid uniqueId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null);
        Task<bool> DeleteEntryAsync(string userId, Guid uniqueId);
        string EncryptPassword(string plainTextPassword);
        string DecryptPassword(string encryptedPassword);
    }
}