using AICharacterChat.Data.Models;

namespace AICharacterChat.Business.Services.Interfaces
{
    /// <summary>
    /// Interface for character service managing character operations
    /// </summary>
    public interface ICharacterService
    {
        /// <summary>
        /// Get all characters
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of characters</returns>
        Task<IEnumerable<Character>> GetAllCharactersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get character by ID
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Character or null if not found</returns>
        Task<Character?> GetCharacterByIdAsync(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get character with conversations
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Character with conversations or null if not found</returns>
        Task<Character?> GetCharacterWithConversationsAsync(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new character
        /// </summary>
        /// <param name="name">Character name</param>
        /// <param name="description">Character description</param>
        /// <param name="systemPrompt">System prompt defining personality</param>
        /// <param name="avatarUrl">Optional avatar URL</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created character</returns>
        Task<Character> CreateCharacterAsync(
            string name,
            string description,
            string systemPrompt,
            string? avatarUrl = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="name">Updated name</param>
        /// <param name="description">Updated description</param>
        /// <param name="systemPrompt">Updated system prompt</param>
        /// <param name="avatarUrl">Updated avatar URL</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated character or null if not found</returns>
        Task<Character?> UpdateCharacterAsync(
            int characterId,
            string name,
            string description,
            string systemPrompt,
            string? avatarUrl = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a character
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deleted successfully</returns>
        Task<bool> DeleteCharacterAsync(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search characters by name or description
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of matching characters</returns>
        Task<IEnumerable<Character>> SearchCharactersAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get recent characters
        /// </summary>
        /// <param name="count">Number of characters to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of recent characters</returns>
        Task<IEnumerable<Character>> GetRecentCharactersAsync(int count = 10, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if character name is available
        /// </summary>
        /// <param name="name">Character name</param>
        /// <param name="excludeCharacterId">Character ID to exclude from check (for updates)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if name is available</returns>
        Task<bool> IsCharacterNameAvailableAsync(string name, int? excludeCharacterId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validate system prompt by testing with Gemini API
        /// </summary>
        /// <param name="systemPrompt">System prompt to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if prompt is valid</returns>
        Task<bool> ValidateSystemPromptAsync(string systemPrompt, CancellationToken cancellationToken = default);
    }
}

