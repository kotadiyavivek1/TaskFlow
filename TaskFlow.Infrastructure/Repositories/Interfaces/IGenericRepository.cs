    using TaskFlow.Domain.Entities;

    namespace TaskFlow.Infrastructure.Repositories.Interfaces;

    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task Delete(T entity);
    }
