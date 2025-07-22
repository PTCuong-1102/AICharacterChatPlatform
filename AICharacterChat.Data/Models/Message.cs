using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AICharacterChat.Data.Models
{
    /// <summary>
    /// Represents a single message in a conversation
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Primary key - auto-incrementing message ID (using bigint for large volume)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageId { get; set; }

        /// <summary>
        /// Foreign key to the conversation this message belongs to
        /// </summary>
        [Required]
        public int ConversationId { get; set; }

        /// <summary>
        /// Sender of the message - either "User" or "AI" (required, max 50 characters)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Sender { get; set; } = string.Empty;

        /// <summary>
        /// Content of the message (required)
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the message was sent (required)
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property - the conversation this message belongs to
        /// </summary>
        [ForeignKey(nameof(ConversationId))]
        public virtual Conversation Conversation { get; set; } = null!;
    }

    /// <summary>
    /// Constants for message senders
    /// </summary>
    public static class MessageSenders
    {
        public const string User = "User";
        public const string AI = "AI";
    }
}

