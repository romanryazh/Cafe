using Cafe.Domain.Entities;
using Cafe.Domain.Interfaces;
using Cafe.Domain.ValueObjects;
using Cafe.Infrastructure.Persistence;
using Cafe.Infrastructure.Persistence.Repositories;
using Cafe.Tests.FakeObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cafe.Tests.Infrastructure.UnitTests;

public class ProductRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    private readonly IFakeObjectDataGenerator<Money> _moneyGenerator;
    private readonly IFakeObjectDataGenerator<Product> _productGenerator;

    public ProductRepositoryTests()
    {
        _moneyGenerator = new MoneyFakeObjectDataGenerator();
        _productGenerator = new ProductFakeObjectDataGenerator(_moneyGenerator);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _productRepository = new ProductRepository(_dbContext);

        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        var product = _productGenerator.GetFaker().Generate();

        await _productRepository.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var result = await _productRepository.GetByIdAsync(product.Id);
        result.Should().NotBeNull();
        result.Name.Should().Be(product.Name);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}