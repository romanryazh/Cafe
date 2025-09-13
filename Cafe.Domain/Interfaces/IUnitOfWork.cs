namespace Cafe.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}