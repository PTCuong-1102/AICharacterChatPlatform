using Microsoft.Extensions.Logging;
using AICharacterChat.Data.Models;
using AICharacterChat.Data.Repositories.Interfaces;
using AICharacterChat.Business.Services.Interfaces;

namespace AICharacterChat.Business.Services
{
    /// <summary>
    /// Chat service implementation managing conversation logic
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeminiApiService _geminiApiService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IUnitOfWork unitOfWork,
            IGeminiApiService geminiApiService,
            ILogger<ChatService> logger)
        {
            _unitOfWork = unitOfWork;
            _geminiApiService = geminiApiService;
            _logger = logger;
        }

        public async Task<(int conversationId, Message aiResponse)> SendMessageAsync(
            int characterId,
            string userMessage,
            int? conversationId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Get or create conversation
                Conversation conversation;
                if (conversationId.HasValue)
                {
                    conversation = await _unitOfWork.Conversations.GetWithCharacterAndMessagesAsync(conversationId.Value)
                        ?? throw new ArgumentException($"Conversation {conversationId} not found");
                }
                else
                {
                    conversation = await StartConversationAsync(characterId, cancellationToken: cancellationToken);
                }

                // Save user message
                var userMessageEntity = new Message
                {
                    ConversationId = conversation.ConversationId,
                    Sender = MessageSenders.User,
                    Content = userMessage,
                    Timestamp = DateTime.UtcNow
                };

                await _unitOfWork.Messages.AddAsync(userMessageEntity);
                await _unitOfWork.SaveChangesAsync();

                // Get conversation history for context
                var conversationHistory = await BuildConversationHistory(conversation.ConversationId);

                // Generate AI response
                var aiResponseText = await _geminiApiService.GenerateResponseAsync(
                    userMessage,
                    conversation.Character.SystemPrompt,
                    conversationHistory,
                    cancellationToken);

                // Save AI response
                var aiMessageEntity = new Message
                {
                    ConversationId = conversation.ConversationId,
                    Sender = MessageSenders.AI,
                    Content = aiResponseText,
                    Timestamp = DateTime.UtcNow
                };

                await _unitOfWork.Messages.AddAsync(aiMessageEntity);
                await _unitOfWork.SaveChangesAsync();

                // Generate conversation title if this is the first exchange
                if (conversation.Messages.Count <= 2 && conversation.Title.StartsWith("Cuộc trò chuyện"))
                {
                    try
                    {
                        var generatedTitle = await GenerateConversationTitleAsync(conversation.ConversationId, cancellationToken);
                        await UpdateConversationTitleAsync(conversation.ConversationId, generatedTitle, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to generate conversation title for conversation {ConversationId}", 
                            conversation.ConversationId);
                    }
                }

                return (conversation.ConversationId, aiMessageEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to character {CharacterId}", characterId);
                throw;
            }
        }

        public async Task<Conversation> StartConversationAsync(
            int characterId,
            string? title = null,
            CancellationToken cancellationToken = default)
        {
            var character = await _unitOfWork.Characters.GetByIdAsync(characterId)
                ?? throw new ArgumentException($"Character {characterId} not found");

            var conversation = new Conversation
            {
                CharacterId = characterId,
                Title = title ?? $"Cuộc trò chuyện với {character.Name}",
                StartedAt = DateTime.UtcNow
            };

            await _unitOfWork.Conversations.AddAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            // Load the character for the returned conversation
            conversation.Character = character;

            return conversation;
        }

        public async Task<Conversation?> GetConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Conversations.GetWithCharacterAndMessagesAsync(conversationId);
        }

        public async Task<IEnumerable<Conversation>> GetConversationsAsync(
            int characterId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Conversations.GetByCharacterIdAsync(characterId);
        }

        public async Task DeleteConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default)
        {
            var conversation = await _unitOfWork.Conversations.GetByIdAsync(conversationId);
            if (conversation != null)
            {
                _unitOfWork.Conversations.Delete(conversation);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UpdateConversationTitleAsync(
            int conversationId,
            string newTitle,
            CancellationToken cancellationToken = default)
        {
            var conversation = await _unitOfWork.Conversations.GetByIdAsync(conversationId);
            if (conversation != null)
            {
                conversation.Title = newTitle;
                _unitOfWork.Conversations.Update(conversation);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateConversationTitleAsync(
            int conversationId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var messages = await _unitOfWork.Messages.GetByConversationIdAsync(conversationId, 0, 4);
                var messageList = messages.ToList();

                if (messageList.Count < 2)
                {
                    return "Cuộc trò chuyện mới";
                }

                // Build context from first few messages
                var context = string.Join("\n", messageList.Select(m => $"{m.Sender}: {m.Content}"));
                
                var titlePrompt = $"Dựa trên cuộc trò chuyện sau, hãy tạo một tiêu đề ngắn gọn (tối đa 50 ký tự) bằng tiếng Việt:\n\n{context}\n\nTiêu đề:";

                var generatedTitle = await _geminiApiService.GenerateResponseAsync(
                    titlePrompt,
                    "Bạn là một trợ lý tạo tiêu đề. Hãy tạo tiêu đề ngắn gọn, súc tích và phù hợp với nội dung cuộc trò chuyện.",
                    null,
                    cancellationToken);

                // Clean up the generated title
                generatedTitle = generatedTitle.Trim().Trim('"').Trim();
                if (generatedTitle.Length > 50)
                {
                    generatedTitle = generatedTitle.Substring(0, 47) + "...";
                }

                return string.IsNullOrEmpty(generatedTitle) ? "Cuộc trò chuyện mới" : generatedTitle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating conversation title for conversation {ConversationId}", conversationId);
                return "Cuộc trò chuyện mới";
            }
        }

        private async Task<List<(string role, string content)>> BuildConversationHistory(int conversationId)
        {
            var messages = await _unitOfWork.Messages.GetRecentForContextAsync(conversationId, 10);
            
            return messages
                .Select(m => (role: m.Sender.ToLower(), content: m.Content))
                .ToList();
        }
    }
}

