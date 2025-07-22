using Microsoft.EntityFrameworkCore;
using AICharacterChat.Data.Models;
using AICharacterChat.Data.Repositories.Interfaces;

namespace AICharacterChat.Data.Repositories
{
    /// <summary>
    /// Conversation repository implementation with specific operations
    /// </summary>
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(ChatDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Conversation>> GetByCharacterIdAsync(int characterId)
        {
            return await _dbSet
                .Where(c => c.CharacterId == characterId)
                .OrderByDescending(c => c.StartedAt)
                .ToListAsync();
        }

        public async Task<Conversation?> GetWithMessagesAsync(int conversationId)
        {
            return await _dbSet
                .Include(c => c.Messages.OrderBy(m => m.Timestamp))
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

        public async Task<Conversation?> GetWithCharacterAndMessagesAsync(int conversationId)
        {
            return await _dbSet
                .Include(c => c.Character)
                .Include(c => c.Messages.OrderBy(m => m.Timestamp))
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

        public async Task<IEnumerable<Conversation>> GetRecentByCharacterAsync(int characterId, int count = 10)
        {
            return await _dbSet
                .Where(c => c.CharacterId == characterId)
                .OrderByDescending(c => c.StartedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conversation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(c => c.StartedAt >= startDate && c.StartedAt <= endDate)
                .OrderByDescending(c => c.StartedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conversation>> SearchByTitleAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(c => c.Title.ToLower().Contains(lowerSearchTerm))
                .OrderByDescending(c => c.StartedAt)
                .ToListAsync();
        }

        public async Task<Conversation?> GetLatestByCharacterAsync(int characterId)
        {
            return await _dbSet
                .Where(c => c.CharacterId == characterId)
                .OrderByDescending(c => c.StartedAt)
                .FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<Conversation>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Character)
                .OrderByDescending(c => c.StartedAt)
                .ToListAsync();
        }
    }
}

