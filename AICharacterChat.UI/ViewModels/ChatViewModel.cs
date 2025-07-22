using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using AICharacterChat.Business.Services.Interfaces;
using AICharacterChat.Data.Models;

namespace AICharacterChat.UI.ViewModels
{
    /// <summary>
    /// Main chat ViewModel managing the chat interface
    /// </summary>
    public partial class ChatViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly ICharacterService _characterService;
        private readonly ILogger<ChatViewModel> _logger;

        [ObservableProperty]
        private ObservableCollection<CharacterViewModel> characters = new();

        [ObservableProperty]
        private ObservableCollection<ConversationViewModel> conversations = new();

        [ObservableProperty]
        private ObservableCollection<MessageViewModel> messages = new();

        [ObservableProperty]
        private CharacterViewModel? selectedCharacter;

        [ObservableProperty]
        private ConversationViewModel? selectedConversation;

        [ObservableProperty]
        private string messageText = string.Empty;

        [ObservableProperty]
        private bool isSending;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string statusMessage = "Sẵn sàng";

        public ChatViewModel(
            IChatService chatService,
            ICharacterService characterService,
            ILogger<ChatViewModel> logger)
        {
            _chatService = chatService;
            _characterService = characterService;
            _logger = logger;
        }

        [RelayCommand]
        private async Task LoadCharactersAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải danh sách nhân vật...";

                var charactersData = await _characterService.GetAllCharactersAsync();
                
                Characters.Clear();
                foreach (var character in charactersData)
                {
                    Characters.Add(new CharacterViewModel(character));
                }

                StatusMessage = $"Đã tải {Characters.Count} nhân vật";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading characters");
                StatusMessage = "Lỗi khi tải danh sách nhân vật";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectCharacterAsync(CharacterViewModel character)
        {
            try
            {
                if (SelectedCharacter != null)
                {
                    SelectedCharacter.IsSelected = false;
                }

                SelectedCharacter = character;
                character.IsSelected = true;

                await LoadConversationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting character {CharacterId}", character.CharacterId);
                StatusMessage = "Lỗi khi chọn nhân vật";
            }
        }

        [RelayCommand]
        private async Task LoadConversationsAsync()
        {
            if (SelectedCharacter == null) return;

            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải cuộc hội thoại...";

                var conversationsData = await _chatService.GetConversationsAsync(SelectedCharacter.CharacterId);
                
                Conversations.Clear();
                foreach (var conversation in conversationsData)
                {
                    Conversations.Add(new ConversationViewModel(conversation));
                }

                StatusMessage = $"Đã tải {Conversations.Count} cuộc hội thoại";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading conversations for character {CharacterId}", SelectedCharacter.CharacterId);
                StatusMessage = "Lỗi khi tải cuộc hội thoại";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectConversationAsync(ConversationViewModel conversation)
        {
            try
            {
                if (SelectedConversation != null)
                {
                    SelectedConversation.IsSelected = false;
                }

                SelectedConversation = conversation;
                conversation.IsSelected = true;

                await LoadMessagesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting conversation {ConversationId}", conversation.ConversationId);
                StatusMessage = "Lỗi khi chọn cuộc hội thoại";
            }
        }

        [RelayCommand]
        private async Task LoadMessagesAsync()
        {
            if (SelectedConversation == null) return;

            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải tin nhắn...";

                var conversationData = await _chatService.GetConversationAsync(SelectedConversation.ConversationId);
                
                Messages.Clear();
                if (conversationData?.Messages != null)
                {
                    foreach (var message in conversationData.Messages)
                    {
                        Messages.Add(new MessageViewModel(message));
                    }
                }

                StatusMessage = $"Đã tải {Messages.Count} tin nhắn";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading messages for conversation {ConversationId}", SelectedConversation.ConversationId);
                StatusMessage = "Lỗi khi tải tin nhắn";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageText) || SelectedCharacter == null || IsSending)
                return;

            try
            {
                IsSending = true;
                StatusMessage = "Đang gửi tin nhắn...";

                var userMessage = MessageText.Trim();
                MessageText = string.Empty;

                // Add user message to UI immediately
                var userMessageViewModel = new MessageViewModel
                {
                    Sender = MessageSenders.User,
                    Content = userMessage,
                    Timestamp = DateTime.UtcNow,
                    ConversationId = SelectedConversation?.ConversationId ?? 0
                };
                Messages.Add(userMessageViewModel);

                // Send message and get AI response
                var (conversationId, aiResponse) = await _chatService.SendMessageAsync(
                    SelectedCharacter.CharacterId,
                    userMessage,
                    SelectedConversation?.ConversationId);

                // Update conversation ID if this was a new conversation
                if (SelectedConversation == null)
                {
                    await LoadConversationsAsync();
                    var newConversation = Conversations.FirstOrDefault(c => c.ConversationId == conversationId);
                    if (newConversation != null)
                    {
                        await SelectConversationAsync(newConversation);
                        return; // LoadMessagesAsync will be called by SelectConversationAsync
                    }
                }

                // Add AI response to UI
                var aiMessageViewModel = new MessageViewModel(aiResponse);
                Messages.Add(aiMessageViewModel);

                StatusMessage = "Tin nhắn đã được gửi";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                StatusMessage = "Lỗi khi gửi tin nhắn";
            }
            finally
            {
                IsSending = false;
            }
        }

        [RelayCommand]
        private async Task StartNewConversationAsync()
        {
            if (SelectedCharacter == null) return;

            try
            {
                if (SelectedConversation != null)
                {
                    SelectedConversation.IsSelected = false;
                }

                SelectedConversation = null;
                Messages.Clear();
                StatusMessage = "Cuộc hội thoại mới đã sẵn sàng";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new conversation");
                StatusMessage = "Lỗi khi tạo cuộc hội thoại mới";
            }
        }

        [RelayCommand]
        private async Task DeleteConversationAsync(ConversationViewModel conversation)
        {
            try
            {
                await _chatService.DeleteConversationAsync(conversation.ConversationId);
                Conversations.Remove(conversation);

                if (SelectedConversation == conversation)
                {
                    SelectedConversation = null;
                    Messages.Clear();
                }

                StatusMessage = "Đã xóa cuộc hội thoại";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting conversation {ConversationId}", conversation.ConversationId);
                StatusMessage = "Lỗi khi xóa cuộc hội thoại";
            }
        }

        public bool CanSendMessage => !string.IsNullOrWhiteSpace(MessageText) && SelectedCharacter != null && !IsSending;
    }
}

