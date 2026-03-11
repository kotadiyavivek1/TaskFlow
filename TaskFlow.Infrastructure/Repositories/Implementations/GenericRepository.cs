using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Infrastructure.Context;
using TaskFlow.Infrastructure.Repositories.Interfaces;

namespace TaskFlow.Infrastructure.Repositories.Implementations;

public class GenericRepository<T>(TaskFlowContext context) : IGenericRepository<T> where T : class
{
    protected readonly TaskFlowContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    // ── Basic CRUD ────────────────────────────────────────────────────────────
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }

    // ── Expression-based queries ──────────────────────────────────────────────
    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);

    // ── Queryable (caller chains Include / Where / OrderBy etc.) ─────────────
    public IQueryable<T> GetQueryable()
        => _dbSet.AsQueryable();

    public IQueryable<T> GetQueryableNoTracking()
        => _dbSet.AsNoTracking().AsQueryable();
}