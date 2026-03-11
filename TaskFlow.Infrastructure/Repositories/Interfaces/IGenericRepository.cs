using System.Linq.Expressions;

namespace TaskFlow.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Full-featured generic repository. Use GetQueryable() for complex queries
/// with Include / Where chaining, and the helper methods for simple lookups.
/// </summary>
public interface IGenericRepository<T> where T : class
{
    // ── Basic CRUD ────────────────────────────────────────────────────────────
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);

    // ── Expression-based queries ──────────────────────────────────────────────
    /// <summary>Return first match or null.</summary>
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Return all matching records.</summary>
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Return true if any record matches the predicate.</summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Return total count of records matching the predicate.</summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    // ── Queryable (for Include, complex ordering, projection, etc.) ───────────
    /// <summary>
    /// Returns an IQueryable that the caller can extend with
    /// .Include(), .Where(), .OrderBy(), .Select() etc. before awaiting.
    /// Use AsNoTracking() for read-only scenarios.
    /// </summary>
    IQueryable<T> GetQueryable();

    /// <summary>Same as GetQueryable but with no-tracking for read-only reads.</summary>
    IQueryable<T> GetQueryableNoTracking();
}
