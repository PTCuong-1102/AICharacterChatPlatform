namespace AICharacterChat.Data.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work interface for managing repositories and transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Character repository
        /// </summary>
        ICharacterRepository Characters { get; }

        /// <summary>
        /// Conversation repository
        /// </summary>
        IConversationRepository Conversations { get; }

        /// <summary>
        /// Message repository
        /// </summary>
        IMessageRepository Messages { get; }

        /// <summary>
        /// Save all changes to the database
        /// </summary>
        /// <returns>Number of affected rows</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begin a database transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}

