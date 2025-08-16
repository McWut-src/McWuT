using McWuT.Data.Models;
using McWuT.Data.Repositories.Base;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace McWuT.Services.PasswordVault
{
    public class PasswordVaultService(
        IUserEntityRepository<PasswordEntry> repository,
        IDataProtectionProvider dataProtectionProvider)
        : BaseUserEntityService<PasswordEntry>(repository), IPasswordVaultService
    {
        private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("PasswordVault");

        public async Task<Guid> CreateEntryAsync(string userId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null)
        {
            var entry = new PasswordEntry
            {
                UniqueId = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                Website = website,
                Username = username,
                EncryptedPassword = !string.IsNullOrEmpty(password) ? EncryptPassword(password) : null,
                Notes = notes,
                Category = category,
                CreatedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await Repository.Create(entry);
            return entry.UniqueId;
        }

        public string DecryptPassword(string encryptedPassword)
        {
            return _dataProtector.Unprotect(encryptedPassword);
        }

        public async Task<bool> DeleteEntryAsync(string userId, Guid uniqueId)
        {
            var entry = await GetEntryAsync(userId, uniqueId);
            if (entry == null)
                return false;

            entry.DeletedDate = DateTime.UtcNow;
            await Repository.Update(entry);
            return true;
        }

        public string EncryptPassword(string plainTextPassword)
        {
            return _dataProtector.Protect(plainTextPassword);
        }

        public async Task<List<PasswordEntry>> GetEntriesAsync(string userId, string? search = null)
        {
            Expression<Func<PasswordEntry, bool>>? filter = pe => pe.UserId == userId && pe.DeletedDate == null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowered = search.ToLower();
                filter = pe =>
                    pe.UserId == userId && pe.DeletedDate == null &&
                    (
                        (pe.Name != null && pe.Name.ToLower().Contains(lowered)) ||
                        (pe.Website != null && pe.Website.ToLower().Contains(lowered)) ||
                        (pe.Username != null && pe.Username.ToLower().Contains(lowered)) ||
                        (pe.Category != null && pe.Category.ToLower().Contains(lowered))
                    );
            }

            var result = await Repository.SearchAll(
                userId,
                filter: filter,
                orderBy: q => q.OrderByDescending(pe => pe.UpdatedAt ?? pe.CreatedAt)
            );
            return result.Items.ToList();
        }

        public async Task<PasswordEntry?> GetEntryAsync(string userId, Guid uniqueId)
        {
            var result = await Repository.SearchAll(
                userId,
                filter: pe => pe.UniqueId == uniqueId && pe.DeletedDate == null
            );

            return result.Items.FirstOrDefault();
        }

        public async Task<bool> UpdateEntryAsync(string userId, Guid uniqueId, string name, string? website = null, string? username = null, string? password = null, string? notes = null, string? category = null)
        {
            var entry = await GetEntryAsync(userId, uniqueId);
            if (entry == null)
                return false;

            entry.Name = name;
            entry.Website = website;
            entry.Username = username;
            if (!string.IsNullOrEmpty(password))
            {
                entry.EncryptedPassword = EncryptPassword(password);
            }
            entry.Notes = notes;
            entry.Category = category;
            entry.UpdatedDate = DateTime.UtcNow;
            entry.UpdatedAt = DateTime.UtcNow;

            await Repository.Update(entry);
            return true;
        }
    }
}