using System;
using CommunityToolkit.Mvvm.ComponentModel;
using AICharacterChat.Data.Models;

namespace AICharacterChat.UI.ViewModels
{
    /// <summary>
    /// ViewModel for displaying chat messages
    /// </summary>
    public partial class MessageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private long messageId;

        [ObservableProperty]
        private int conversationId;

        [ObservableProperty]
        private string sender = string.Empty;

        [ObservableProperty]
        private string content = string.Empty;

        [ObservableProperty]
        private DateTime timestamp;

        [ObservableProperty]
        private bool isFromUser;

        [ObservableProperty]
        private bool isFromAI;

        public MessageViewModel()
        {
        }

        public MessageViewModel(Message message)
        {
            MessageId = message.MessageId;
            ConversationId = message.ConversationId;
            Sender = message.Sender;
            Content = message.Content;
            Timestamp = message.Timestamp;
            
            UpdateSenderFlags();
        }

        partial void OnSenderChanged(string value)
        {
            UpdateSenderFlags();
        }

        private void UpdateSenderFlags()
        {
            IsFromUser = Sender.Equals(MessageSenders.User, StringComparison.OrdinalIgnoreCase);
            IsFromAI = Sender.Equals(MessageSenders.AI, StringComparison.OrdinalIgnoreCase);
        }

        public Message ToModel()
        {
            return new Message
            {
                MessageId = MessageId,
                ConversationId = ConversationId,
                Sender = Sender,
                Content = Content,
                Timestamp = Timestamp
            };
        }

        public string FormattedTimestamp => Timestamp.ToString("HH:mm");
        public string FormattedDate => Timestamp.ToString("dd/MM/yyyy");
        public string SenderDisplayName => IsFromUser ? "Bạn" : "AI";
    }
}

