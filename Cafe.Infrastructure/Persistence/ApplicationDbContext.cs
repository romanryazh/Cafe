using Cafe.Application.Common.Interfaces;
using Cafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Product> Products { get; }
    public DbSet<Order> Orders { get; }
    
    public DbSet<T> Set<T>() where T : class => base.Set<T>();

    public override void Dispose()
    {
        base.Dispose();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureAssemblyReference.Assembly);
        base.OnModelCreating(modelBuilder);
    }

}