using Cafe.Domain.Entities;
using Cafe.Domain.Enums;

namespace Cafe.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyCollection<Product>> GetByCategoryAsync(ProductCategory category,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Product>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);
}