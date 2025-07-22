using System.Linq.Expressions;

namespace AICharacterChat.Data.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface defining common CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity or null if not found</returns>
        Task<T?> GetByIdAsync(object id);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Find entities matching the specified condition
        /// </summary>
        /// <param name="expression">Filter expression</param>
        /// <returns>Collection of matching entities</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get first entity matching the specified condition
        /// </summary>
        /// <param name="expression">Filter expression</param>
        /// <returns>First matching entity or null</returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>Added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Add multiple entities
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        Task DeleteByIdAsync(object id);

        /// <summary>
        /// Delete multiple entities
        /// </summary>
        /// <param name="entities">Entities to delete</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Check if any entity matches the specified condition
        /// </summary>
        /// <param name="expression">Filter expression</param>
        /// <returns>True if any entity matches</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Count entities matching the specified condition
        /// </summary>
        /// <param name="expression">Filter expression</param>
        /// <returns>Count of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
    }
}

