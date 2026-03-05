using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Infrastructure.Context;
using TaskFlow.Infrastructure.Repositories.Interfaces;

namespace TaskFlow.Infrastructure.Repositories.Implementations;

public class GenericRepository<T>(TaskFlowContext context) : IGenericRepository<T> where T : class
{
    protected readonly TaskFlowContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
         await SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}