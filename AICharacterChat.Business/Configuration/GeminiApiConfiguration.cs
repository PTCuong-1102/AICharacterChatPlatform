namespace AICharacterChat.Business.Configuration
{
    /// <summary>
    /// Configuration settings for Google Gemini API
    /// </summary>
    public class GeminiApiConfiguration
    {
        /// <summary>
        /// Configuration section name in appsettings.json
        /// </summary>
        public const string SectionName = "GeminiApi";

        /// <summary>
        /// Google Gemini API key
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Base URL for Gemini API
        /// </summary>
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";

        /// <summary>
        /// Model name to use (e.g., gemini-pro, gemini-pro-vision)
        /// </summary>
        public string Model { get; set; } = "gemini-pro";

        /// <summary>
        /// Maximum tokens for response
        /// </summary>
        public int MaxTokens { get; set; } = 2048;

        /// <summary>
        /// Temperature for response generation (0.0 to 1.0)
        /// </summary>
        public double Temperature { get; set; } = 0.7;

        /// <summary>
        /// Top-p for nucleus sampling
        /// </summary>
        public double TopP { get; set; } = 0.8;

        /// <summary>
        /// Top-k for top-k sampling
        /// </summary>
        public int TopK { get; set; } = 40;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Maximum number of context messages to include
        /// </summary>
        public int MaxContextMessages { get; set; } = 10;
    }
}

