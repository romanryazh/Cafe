using Cafe.Application.Common.Interfaces;
using Cafe.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Infrastructure.Persistence;

public class UnitOfWork(IApplicationDbContext context, IServiceProvider serviceProvider) : IUnitOfWork
{
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;

    public IProductRepository ProductRepository => _productRepository ??= 
        serviceProvider.GetRequiredService<IProductRepository>();
    public IOrderRepository OrderRepository => _orderRepository ??=
        serviceProvider.GetRequiredService<IOrderRepository>();
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    { 
        context.Dispose();
    }
}