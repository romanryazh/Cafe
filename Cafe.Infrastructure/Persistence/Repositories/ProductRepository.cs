using Cafe.Application.Common.Interfaces;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Persistence.Repositories;

public class ProductRepository(IApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetByCategoryAsync(ProductCategory category,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(p => p.Name
                .Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase), cancellationToken);
    }
}