using Microsoft.EntityFrameworkCore;
using AICharacterChat.Data.Models;
using AICharacterChat.Data.Repositories.Interfaces;

namespace AICharacterChat.Data.Repositories
{
    /// <summary>
    /// Message repository implementation with specific operations
    /// </summary>
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId, int skip, int take)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetBySenderAsync(int conversationId, string sender)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId && m.Sender == sender)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetByTimeRangeAsync(int conversationId, DateTime startTime, DateTime endTime)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId && 
                           m.Timestamp >= startTime && 
                           m.Timestamp <= endTime)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<Message?> GetLatestByConversationAsync(int conversationId)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Message>> SearchByContentAsync(int conversationId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetByConversationIdAsync(conversationId);
            }

            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(m => m.ConversationId == conversationId && 
                           m.Content.ToLower().Contains(lowerSearchTerm))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<int> GetMessageCountAsync(int conversationId)
        {
            return await _dbSet
                .CountAsync(m => m.ConversationId == conversationId);
        }

        public async Task<IEnumerable<Message>> GetRecentForContextAsync(int conversationId, int count = 10)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .OrderBy(m => m.Timestamp) // Re-order chronologically for context
                .ToListAsync();
        }
    }
}

