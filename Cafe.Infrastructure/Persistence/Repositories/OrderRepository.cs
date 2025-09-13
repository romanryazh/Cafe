using Cafe.Application.Common.Interfaces;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Persistence.Repositories;

public class OrderRepository(IApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<IReadOnlyCollection<Order>> GetByStatusAsync(OrderStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<Order>> GetByCustomerAsync(string customerName,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.CustomerName == customerName)
            .ToListAsync(cancellationToken);
    }
}