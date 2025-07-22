using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using AICharacterChat.Data.Models;

namespace AICharacterChat.UI.ViewModels
{
    /// <summary>
    /// ViewModel for displaying conversation information
    /// </summary>
    public partial class ConversationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int conversationId;

        [ObservableProperty]
        private int characterId;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private DateTime startedAt;

        [ObservableProperty]
        private CharacterViewModel? character;

        [ObservableProperty]
        private ObservableCollection<MessageViewModel> messages = new();

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private int messageCount;

        [ObservableProperty]
        private string? lastMessage;

        [ObservableProperty]
        private DateTime? lastMessageTime;

        public ConversationViewModel()
        {
        }

        public ConversationViewModel(Conversation conversation)
        {
            ConversationId = conversation.ConversationId;
            CharacterId = conversation.CharacterId;
            Title = conversation.Title;
            StartedAt = conversation.StartedAt;

            if (conversation.Character != null)
            {
                Character = new CharacterViewModel(conversation.Character);
            }

            if (conversation.Messages?.Any() == true)
            {
                Messages = new ObservableCollection<MessageViewModel>(
                    conversation.Messages.Select(m => new MessageViewModel(m)));
                
                UpdateMessageInfo();
            }
        }

        public void AddMessage(MessageViewModel message)
        {
            Messages.Add(message);
            UpdateMessageInfo();
        }

        public void UpdateFromModel(Conversation conversation)
        {
            ConversationId = conversation.ConversationId;
            CharacterId = conversation.CharacterId;
            Title = conversation.Title;
            StartedAt = conversation.StartedAt;

            if (conversation.Character != null)
            {
                Character = new CharacterViewModel(conversation.Character);
            }

            if (conversation.Messages?.Any() == true)
            {
                Messages.Clear();
                foreach (var message in conversation.Messages)
                {
                    Messages.Add(new MessageViewModel(message));
                }
                UpdateMessageInfo();
            }
        }

        private void UpdateMessageInfo()
        {
            MessageCount = Messages.Count;
            
            var lastMsg = Messages.LastOrDefault();
            if (lastMsg != null)
            {
                LastMessage = lastMsg.Content.Length > 50 
                    ? lastMsg.Content.Substring(0, 47) + "..."
                    : lastMsg.Content;
                LastMessageTime = lastMsg.Timestamp;
            }
        }

        public Conversation ToModel()
        {
            var conversation = new Conversation
            {
                ConversationId = ConversationId,
                CharacterId = CharacterId,
                Title = Title,
                StartedAt = StartedAt
            };

            if (Character != null)
            {
                conversation.Character = Character.ToModel();
            }

            conversation.Messages = Messages.Select(m => m.ToModel()).ToList();

            return conversation;
        }

        public string FormattedStartTime => StartedAt.ToString("dd/MM/yyyy HH:mm");
        public string FormattedLastMessageTime => LastMessageTime?.ToString("HH:mm") ?? "";
        public string CharacterName => Character?.Name ?? "Unknown";
    }
}

