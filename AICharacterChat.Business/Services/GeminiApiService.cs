using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AICharacterChat.Business.Configuration;
using AICharacterChat.Business.Models;
using AICharacterChat.Business.Services.Interfaces;

namespace AICharacterChat.Business.Services
{
    /// <summary>
    /// Google Gemini API service implementation
    /// </summary>
    public class GeminiApiService : IGeminiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiApiConfiguration _config;
        private readonly ILogger<GeminiApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public GeminiApiService(
            HttpClient httpClient,
            IOptions<GeminiApiConfiguration> config,
            ILogger<GeminiApiService> logger)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            // Configure HttpClient
            _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
            _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);
        }

        public async Task<string> GenerateResponseAsync(
            string prompt,
            string systemPrompt,
            List<(string role, string content)>? conversationHistory = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = BuildGeminiRequest(prompt, systemPrompt, conversationHistory);
                var response = await GenerateResponseAsync(request, cancellationToken);

                if (response.Candidates?.Count > 0 && 
                    response.Candidates[0].Content?.Parts?.Count > 0)
                {
                    return response.Candidates[0].Content.Parts[0].Text;
                }

                _logger.LogWarning("No valid response received from Gemini API");
                return "Xin lỗi, tôi không thể tạo phản hồi lúc này. Vui lòng thử lại.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating response from Gemini API");
                return "Xin lỗi, đã xảy ra lỗi khi xử lý yêu cầu của bạn. Vui lòng thử lại sau.";
            }
        }

        public async Task<GeminiResponse> GenerateResponseAsync(
            GeminiRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                throw new InvalidOperationException("Gemini API key is not configured");
            }

            var url = $"v1beta/models/{_config.Model}:generateContent?key={_config.ApiKey}";
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var fullUrl = new Uri(_httpClient.BaseAddress, url);
            _logger.LogInformation("Sending request to Gemini API: {Url} (Full URL: {FullUrl})", url, fullUrl);
            _logger.LogInformation("Request body: {Json}", json);

            try 
            {
                var response = await _httpClient.PostAsync(url, content, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("Response status: {StatusCode}, Content: {Content}", 
                    response.StatusCode, responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API error: {StatusCode} - {Content}", 
                    response.StatusCode, responseContent);

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<GeminiErrorResponse>(responseContent, _jsonOptions);
                    throw new HttpRequestException(
                        $"Gemini API error: {errorResponse?.Error?.Message ?? "Unknown error"}");
                }
                catch (JsonException)
                {
                    throw new HttpRequestException($"Gemini API error: {response.StatusCode}");
                }
            }

                var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, _jsonOptions);
                return geminiResponse ?? new GeminiResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP request failed to Gemini API");
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var testRequest = new GeminiRequest
                {
                    Contents = new List<GeminiContent>
                    {
                        new GeminiContent
                        {
                            Parts = new List<GeminiPart>
                            {
                                new GeminiPart { Text = "Hello" }
                            }
                        }
                    }
                };

                var response = await GenerateResponseAsync(testRequest, cancellationToken);
                return response.Candidates?.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to test Gemini API connection");
                return false;
            }
        }

        public async Task<List<string>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_config.ApiKey))
                {
                    throw new InvalidOperationException("Gemini API key is not configured");
                }

                var url = $"v1beta/models?key={_config.ApiKey}";
                var response = await _httpClient.GetAsync(url, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    // Parse models response (implementation depends on actual API response format)
                    // For now, return default models
                    return new List<string> { "gemini-pro", "gemini-pro-vision" };
                }

                _logger.LogWarning("Failed to get available models: {StatusCode}", response.StatusCode);
                return new List<string> { _config.Model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available models");
                return new List<string> { _config.Model };
            }
        }

        private GeminiRequest BuildGeminiRequest(
            string prompt,
            string systemPrompt,
            List<(string role, string content)>? conversationHistory)
        {
            var contents = new List<GeminiContent>();

            // Add system prompt as the first message
            if (!string.IsNullOrEmpty(systemPrompt))
            {
                contents.Add(new GeminiContent
                {
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = systemPrompt }
                    },
                    Role = GeminiConstants.UserRole
                });

                // Add a model acknowledgment
                contents.Add(new GeminiContent
                {
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = "Tôi hiểu và sẽ tuân theo hướng dẫn này." }
                    },
                    Role = GeminiConstants.ModelRole
                });
            }

            // Add conversation history
            if (conversationHistory != null)
            {
                var recentHistory = conversationHistory
                    .TakeLast(_config.MaxContextMessages)
                    .ToList();

                foreach (var (role, content) in recentHistory)
                {
                    var geminiRole = role.ToLower() == "user" ? 
                        GeminiConstants.UserRole : GeminiConstants.ModelRole;

                    contents.Add(new GeminiContent
                    {
                        Parts = new List<GeminiPart>
                        {
                            new GeminiPart { Text = content }
                        },
                        Role = geminiRole
                    });
                }
            }

            // Add current user prompt
            contents.Add(new GeminiContent
            {
                Parts = new List<GeminiPart>
                {
                    new GeminiPart { Text = prompt }
                },
                Role = GeminiConstants.UserRole
            });

            return new GeminiRequest
            {
                Contents = contents,
                GenerationConfig = new GeminiGenerationConfig
                {
                    Temperature = _config.Temperature,
                    TopK = _config.TopK,
                    TopP = _config.TopP,
                    MaxOutputTokens = _config.MaxTokens
                },
                SafetySettings = new List<GeminiSafetySetting>
                {
                    new GeminiSafetySetting
                    {
                        Category = GeminiConstants.SafetyCategories.HarmCategoryHarassment,
                        Threshold = GeminiConstants.SafetyThresholds.BlockMediumAndAbove
                    },
                    new GeminiSafetySetting
                    {
                        Category = GeminiConstants.SafetyCategories.HarmCategoryHateSpeech,
                        Threshold = GeminiConstants.SafetyThresholds.BlockMediumAndAbove
                    },
                    new GeminiSafetySetting
                    {
                        Category = GeminiConstants.SafetyCategories.HarmCategorySexuallyExplicit,
                        Threshold = GeminiConstants.SafetyThresholds.BlockMediumAndAbove
                    },
                    new GeminiSafetySetting
                    {
                        Category = GeminiConstants.SafetyCategories.HarmCategoryDangerousContent,
                        Threshold = GeminiConstants.SafetyThresholds.BlockMediumAndAbove
                    }
                }
            };
        }
    }
}

