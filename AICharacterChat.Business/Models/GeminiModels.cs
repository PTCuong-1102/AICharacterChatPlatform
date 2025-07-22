using System.Text.Json.Serialization;

namespace AICharacterChat.Business.Models
{
    /// <summary>
    /// Request model for Gemini API
    /// </summary>
    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<GeminiContent> Contents { get; set; } = new();

        [JsonPropertyName("generationConfig")]
        public GeminiGenerationConfig? GenerationConfig { get; set; }

        [JsonPropertyName("safetySettings")]
        public List<GeminiSafetySetting>? SafetySettings { get; set; }
    }

    /// <summary>
    /// Content part of Gemini request
    /// </summary>
    public class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart> Parts { get; set; } = new();

        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }

    /// <summary>
    /// Part of Gemini content
    /// </summary>
    public class GeminiPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    /// <summary>
    /// Generation configuration for Gemini API
    /// </summary>
    public class GeminiGenerationConfig
    {
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("topK")]
        public int? TopK { get; set; }

        [JsonPropertyName("topP")]
        public double? TopP { get; set; }

        [JsonPropertyName("maxOutputTokens")]
        public int? MaxOutputTokens { get; set; }

        [JsonPropertyName("stopSequences")]
        public List<string>? StopSequences { get; set; }
    }

    /// <summary>
    /// Safety setting for Gemini API
    /// </summary>
    public class GeminiSafetySetting
    {
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("threshold")]
        public string Threshold { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model from Gemini API
    /// </summary>
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate> Candidates { get; set; } = new();

        [JsonPropertyName("promptFeedback")]
        public GeminiPromptFeedback? PromptFeedback { get; set; }
    }

    /// <summary>
    /// Candidate response from Gemini
    /// </summary>
    public class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent? Content { get; set; }

        [JsonPropertyName("finishReason")]
        public string? FinishReason { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("safetyRatings")]
        public List<GeminiSafetyRating>? SafetyRatings { get; set; }
    }

    /// <summary>
    /// Prompt feedback from Gemini
    /// </summary>
    public class GeminiPromptFeedback
    {
        [JsonPropertyName("safetyRatings")]
        public List<GeminiSafetyRating>? SafetyRatings { get; set; }
    }

    /// <summary>
    /// Safety rating from Gemini
    /// </summary>
    public class GeminiSafetyRating
    {
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("probability")]
        public string Probability { get; set; } = string.Empty;
    }

    /// <summary>
    /// Error response from Gemini API
    /// </summary>
    public class GeminiErrorResponse
    {
        [JsonPropertyName("error")]
        public GeminiError? Error { get; set; }
    }

    /// <summary>
    /// Error details from Gemini API
    /// </summary>
    public class GeminiError
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Constants for Gemini API
    /// </summary>
    public static class GeminiConstants
    {
        public const string UserRole = "user";
        public const string ModelRole = "model";
        
        public static class SafetyCategories
        {
            public const string HarmCategoryHarassment = "HARM_CATEGORY_HARASSMENT";
            public const string HarmCategoryHateSpeech = "HARM_CATEGORY_HATE_SPEECH";
            public const string HarmCategorySexuallyExplicit = "HARM_CATEGORY_SEXUALLY_EXPLICIT";
            public const string HarmCategoryDangerousContent = "HARM_CATEGORY_DANGEROUS_CONTENT";
        }

        public static class SafetyThresholds
        {
            public const string BlockNone = "BLOCK_NONE";
            public const string BlockLowAndAbove = "BLOCK_LOW_AND_ABOVE";
            public const string BlockMediumAndAbove = "BLOCK_MEDIUM_AND_ABOVE";
            public const string BlockOnlyHigh = "BLOCK_ONLY_HIGH";
        }
    }
}

