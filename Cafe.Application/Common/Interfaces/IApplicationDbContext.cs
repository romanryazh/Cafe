using Cafe.Domain.Entities;
using Cafe.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<T> Set<T>() where T : class;
    void Dispose();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}