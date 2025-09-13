using Cafe.Domain.Entities;
using Cafe.Domain.Enums;

namespace Cafe.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IReadOnlyCollection<Order>> GetByStatusAsync(OrderStatus status, 
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Order>> GetByCustomerAsync(string customerName,
        CancellationToken cancellationToken = default);
}