using McWuT.Data.Contexts;
using McWuT.Data.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace McWuT.Data.Services
{
    public class PasswordVaultService : IPasswordVaultService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;

        public PasswordVaultService(ApplicationDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("PasswordVault.Passwords");
        }

        public async Task<List<PasswordEntry>> GetEntriesAsync(string userId, string? search = null)
        {
            var query = _context.PasswordEntries
                .Where(pe => pe.UserId == userId && pe.DeletedDate == null);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.ToLower();
                query = query.Where(pe => 
                    pe.Name.ToLower().Contains(searchTerm) ||
                    (pe.Website != null && pe.Website.ToLower().Contains(searchTerm)) ||
                    (pe.Username != null && pe.Username.ToLower().Contains(searchTerm)) ||
                    (pe.Category != null && pe.Category.ToLower().Contains(searchTerm)));
            }

            return await query
                .OrderByDescending(pe => pe.UpdatedAt ?? pe.CreatedAt)
                .ToListAsync();
        }

        public async Task<PasswordEntry?> GetEntryAsync(string userId, Guid uniqueId)
        {
            return await _context.PasswordEntries
                .FirstOrDefaultAsync(pe => pe.UserId == userId && pe.UniqueId == uniqueId && pe.DeletedDate == null);
        }

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

            _context.PasswordEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry.UniqueId;
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

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntryAsync(string userId, Guid uniqueId)
        {
            var entry = await GetEntryAsync(userId, uniqueId);
            if (entry == null)
                return false;

            entry.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public string EncryptPassword(string plainTextPassword)
        {
            return _dataProtector.Protect(plainTextPassword);
        }

        public string DecryptPassword(string encryptedPassword)
        {
            return _dataProtector.Unprotect(encryptedPassword);
        }
    }
}