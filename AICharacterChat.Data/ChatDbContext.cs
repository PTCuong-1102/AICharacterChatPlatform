using Microsoft.EntityFrameworkCore;
using AICharacterChat.Data.Models;

namespace AICharacterChat.Data
{
    /// <summary>
    /// Database context for the AI Character Chat application
    /// </summary>
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Characters table
        /// </summary>
        public DbSet<Character> Characters { get; set; }

        /// <summary>
        /// Conversations table
        /// </summary>
        public DbSet<Conversation> Conversations { get; set; }

        /// <summary>
        /// Messages table
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Character entity
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.CharacterId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.SystemPrompt).IsRequired();
                entity.Property(e => e.AvatarUrl).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).IsRequired();

                // Index for performance
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.CreatedAt);
            });

            // Configure Conversation entity
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.ConversationId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartedAt).IsRequired();

                // Foreign key relationship
                entity.HasOne(e => e.Character)
                      .WithMany(e => e.Conversations)
                      .HasForeignKey(e => e.CharacterId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Indexes for performance
                entity.HasIndex(e => e.CharacterId);
                entity.HasIndex(e => e.StartedAt);
            });

            // Configure Message entity
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.Property(e => e.Sender).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();

                // Foreign key relationship
                entity.HasOne(e => e.Conversation)
                      .WithMany(e => e.Messages)
                      .HasForeignKey(e => e.ConversationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Indexes for performance
                entity.HasIndex(e => e.ConversationId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.Sender);
            });

            // Seed some default data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default characters
            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    CharacterId = 1,
                    Name = "Trợ lý AI thông minh",
                    Description = "Một trợ lý AI thông minh và hữu ích, sẵn sàng giúp đỡ bạn với mọi câu hỏi.",
                    SystemPrompt = "Bạn là một trợ lý AI thông minh và hữu ích. Hãy trả lời các câu hỏi một cách chính xác, chi tiết và thân thiện. Luôn sử dụng tiếng Việt để giao tiếp trừ khi được yêu cầu khác.",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Character
                {
                    CharacterId = 2,
                    Name = "Chuyên gia lập trình",
                    Description = "Chuyên gia lập trình với kinh nghiệm sâu rộng về nhiều ngôn ngữ và công nghệ.",
                    SystemPrompt = "Bạn là một chuyên gia lập trình có kinh nghiệm sâu rộng về nhiều ngôn ngữ lập trình như C#, Python, JavaScript, và các framework hiện đại. Hãy giúp đỡ người dùng với các vấn đề về lập trình, giải thích code, và đưa ra lời khuyên tốt nhất về best practices.",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

