using AICharacterChat.Business.Models;

namespace AICharacterChat.Business.Services.Interfaces
{
    /// <summary>
    /// Interface for Google Gemini API service
    /// </summary>
    public interface IGeminiApiService
    {
        /// <summary>
        /// Generate response from Gemini API
        /// </summary>
        /// <param name="prompt">User prompt</param>
        /// <param name="systemPrompt">System prompt for character personality</param>
        /// <param name="conversationHistory">Previous conversation messages for context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Generated response</returns>
        Task<string> GenerateResponseAsync(
            string prompt, 
            string systemPrompt, 
            List<(string role, string content)>? conversationHistory = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate response with custom configuration
        /// </summary>
        /// <param name="request">Gemini request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Gemini response</returns>
        Task<GeminiResponse> GenerateResponseAsync(
            GeminiRequest request, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Test API connection and authentication
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if connection is successful</returns>
        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get available models
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of available models</returns>
        Task<List<string>> GetAvailableModelsAsync(CancellationToken cancellationToken = default);
    }
}

