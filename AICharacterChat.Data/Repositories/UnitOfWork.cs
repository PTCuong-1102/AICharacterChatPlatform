using Microsoft.EntityFrameworkCore.Storage;
using AICharacterChat.Data.Repositories.Interfaces;

namespace AICharacterChat.Data.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing repositories and transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _context;
        private IDbContextTransaction? _transaction;

        private ICharacterRepository? _characters;
        private IConversationRepository? _conversations;
        private IMessageRepository? _messages;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
        }

        public ICharacterRepository Characters
        {
            get
            {
                _characters ??= new CharacterRepository(_context);
                return _characters;
            }
        }

        public IConversationRepository Conversations
        {
            get
            {
                _conversations ??= new ConversationRepository(_context);
                return _conversations;
            }
        }

        public IMessageRepository Messages
        {
            get
            {
                _messages ??= new MessageRepository(_context);
                return _messages;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

