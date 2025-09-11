using AutoFixture;
using Cafe.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Cafe.Tests.Domain.UnitTests;

public class OrderItemTests
{
    private readonly IFixture _fixture;

    public OrderItemTests()
    {
        _fixture = new Fixture();

        _fixture.Customize(new MoneyCustomization()).Customize(new ProductCustomization());
    }

    [Fact]
    public void Create_WithValidParameters_ShouldSucceed()
    {
        var product = _fixture.Create<Product>();
        var quantity = _fixture.Create<int>();

        var orderItem = OrderItem.Create(product.Id, product.Name, product.Price, quantity);

        orderItem.ProductId.Should().Be(product.Id);
        orderItem.ProductName.Should().Be(product.Name);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.UnitPrice.Should().Be(product.Price);
        orderItem.TotalPrice.Should().Be(product.Price * quantity);
    }
}