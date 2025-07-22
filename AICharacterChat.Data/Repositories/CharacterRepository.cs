using Microsoft.EntityFrameworkCore;
using AICharacterChat.Data.Models;
using AICharacterChat.Data.Repositories.Interfaces;

namespace AICharacterChat.Data.Repositories
{
    /// <summary>
    /// Character repository implementation with specific operations
    /// </summary>
    public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
    {
        public CharacterRepository(ChatDbContext context) : base(context)
        {
        }

        public async Task<Character?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Character>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Character?> GetWithConversationsAsync(int characterId)
        {
            return await _dbSet
                .Include(c => c.Conversations)
                .ThenInclude(conv => conv.Messages)
                .FirstOrDefaultAsync(c => c.CharacterId == characterId);
        }

        public async Task<IEnumerable<Character>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(c => c.Name.ToLower().Contains(lowerSearchTerm) ||
                           (c.Description != null && c.Description.ToLower().Contains(lowerSearchTerm)))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Character>> GetRecentAsync(int count = 10)
        {
            return await _dbSet
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Character>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}

