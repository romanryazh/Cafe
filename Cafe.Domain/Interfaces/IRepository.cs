using Cafe.Domain.Entities;

namespace Cafe.Domain.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}