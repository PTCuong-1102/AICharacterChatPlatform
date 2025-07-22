using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AICharacterChat.Data.Models
{
    /// <summary>
    /// Represents an AI character with personality and system prompt
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Primary key - auto-incrementing character ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }

        /// <summary>
        /// Name of the character (required, max 100 characters)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Short description of the character (optional, max 500 characters)
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// System prompt defining character's personality and knowledge (required)
        /// </summary>
        [Required]
        public string SystemPrompt { get; set; } = string.Empty;

        /// <summary>
        /// URL path to character's avatar image (optional, max 255 characters)
        /// </summary>
        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Date and time when the character was created (required)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property - conversations with this character
        /// </summary>
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}

