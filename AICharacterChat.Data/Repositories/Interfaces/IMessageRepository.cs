using AICharacterChat.Data.Models;

namespace AICharacterChat.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Message entity with specific operations
    /// </summary>
    public interface IMessageRepository : IGenericRepository<Message>
    {
        /// <summary>
        /// Get messages by conversation ID
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <returns>Collection of messages ordered by timestamp</returns>
        Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId);

        /// <summary>
        /// Get messages by conversation ID with pagination
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="skip">Number of messages to skip</param>
        /// <param name="take">Number of messages to take</param>
        /// <returns>Collection of messages</returns>
        Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId, int skip, int take);

        /// <summary>
        /// Get messages by sender type
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="sender">Sender type (User or AI)</param>
        /// <returns>Collection of messages from specified sender</returns>
        Task<IEnumerable<Message>> GetBySenderAsync(int conversationId, string sender);

        /// <summary>
        /// Get messages within a time range
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>Collection of messages</returns>
        Task<IEnumerable<Message>> GetByTimeRangeAsync(int conversationId, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Get the latest message in a conversation
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <returns>Latest message or null</returns>
        Task<Message?> GetLatestByConversationAsync(int conversationId);

        /// <summary>
        /// Search messages by content
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Collection of matching messages</returns>
        Task<IEnumerable<Message>> SearchByContentAsync(int conversationId, string searchTerm);

        /// <summary>
        /// Get message count for a conversation
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <returns>Number of messages</returns>
        Task<int> GetMessageCountAsync(int conversationId);

        /// <summary>
        /// Get recent messages for building context
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="count">Number of recent messages to return</param>
        /// <returns>Collection of recent messages ordered by timestamp</returns>
        Task<IEnumerable<Message>> GetRecentForContextAsync(int conversationId, int count = 10);
    }
}

