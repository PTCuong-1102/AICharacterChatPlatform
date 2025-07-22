using AICharacterChat.Data.Models;

namespace AICharacterChat.Business.Services.Interfaces
{
    /// <summary>
    /// Interface for chat service managing conversation logic
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Send a message and get AI response
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="userMessage">User message</param>
        /// <param name="conversationId">Optional conversation ID (null for new conversation)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple of conversation ID and AI response message</returns>
        Task<(int conversationId, Message aiResponse)> SendMessageAsync(
            int characterId,
            string userMessage,
            int? conversationId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Start a new conversation with a character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="title">Optional conversation title</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>New conversation</returns>
        Task<Conversation> StartConversationAsync(
            int characterId,
            string? title = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get conversation with messages
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Conversation with messages</returns>
        Task<Conversation?> GetConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get conversations for a character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of conversations</returns>
        Task<IEnumerable<Conversation>> GetConversationsAsync(
            int characterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a conversation
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update conversation title
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="newTitle">New title</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateConversationTitleAsync(
            int conversationId,
            string newTitle,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate a conversation title based on the first few messages
        /// </summary>
        /// <param name="conversationId">Conversation ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Generated title</returns>
        Task<string> GenerateConversationTitleAsync(
            int conversationId,
            CancellationToken cancellationToken = default);
    }
}

