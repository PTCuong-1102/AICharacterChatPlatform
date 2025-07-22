using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AICharacterChat.Data.Models
{
    /// <summary>
    /// Represents a conversation session with an AI character
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Primary key - auto-incrementing conversation ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversationId { get; set; }

        /// <summary>
        /// Foreign key to the character involved in this conversation
        /// </summary>
        [Required]
        public int CharacterId { get; set; }

        /// <summary>
        /// Title of the conversation (required, max 200 characters)
        /// Can be auto-generated or user-defined
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the conversation started (required)
        /// </summary>
        [Required]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property - the character involved in this conversation
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public virtual Character Character { get; set; } = null!;

        /// <summary>
        /// Navigation property - messages in this conversation
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}

