using AICharacterChat.Data.Models;

namespace AICharacterChat.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Conversation entity with specific operations
    /// </summary>
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        /// <summary>
        /// Get conversations by character ID
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <returns>Collection of conversations</returns>
        Task<IEnumerable<Conversation>> GetByCharacterIdAsync(int characterId);

        /// <summary>
        /// Get conversation with all messages
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <returns>Conversation with messages or null</returns>
        Task<Conversation?> GetWithMessagesAsync(int conversationId);

        /// <summary>
        /// Get conversation with character and messages
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <returns>Conversation with character and messages or null</returns>
        Task<Conversation?> GetWithCharacterAndMessagesAsync(int conversationId);

        /// <summary>
        /// Get recent conversations for a character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="count">Number of conversations to return</param>
        /// <returns>Collection of recent conversations</returns>
        Task<IEnumerable<Conversation>> GetRecentByCharacterAsync(int characterId, int count = 10);

        /// <summary>
        /// Get conversations started within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Collection of conversations</returns>
        Task<IEnumerable<Conversation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Search conversations by title
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Collection of matching conversations</returns>
        Task<IEnumerable<Conversation>> SearchByTitleAsync(string searchTerm);

        /// <summary>
        /// Get the latest conversation for a character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <returns>Latest conversation or null</returns>
        Task<Conversation?> GetLatestByCharacterAsync(int characterId);
    }
}

