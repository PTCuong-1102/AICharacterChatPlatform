using AICharacterChat.Data.Models;

namespace AICharacterChat.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Character entity with specific operations
    /// </summary>
    public interface ICharacterRepository : IGenericRepository<Character>
    {
        /// <summary>
        /// Get character by name
        /// </summary>
        /// <param name="name">Character name</param>
        /// <returns>Character or null if not found</returns>
        Task<Character?> GetByNameAsync(string name);

        /// <summary>
        /// Get characters created within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Collection of characters</returns>
        Task<IEnumerable<Character>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get character with all conversations
        /// </summary>
        /// <param name="characterId">Character ID</param>
        /// <returns>Character with conversations or null</returns>
        Task<Character?> GetWithConversationsAsync(int characterId);

        /// <summary>
        /// Search characters by name or description
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Collection of matching characters</returns>
        Task<IEnumerable<Character>> SearchAsync(string searchTerm);

        /// <summary>
        /// Get most recently created characters
        /// </summary>
        /// <param name="count">Number of characters to return</param>
        /// <returns>Collection of recent characters</returns>
        Task<IEnumerable<Character>> GetRecentAsync(int count = 10);
    }
}

