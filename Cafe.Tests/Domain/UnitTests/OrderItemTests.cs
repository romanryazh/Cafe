using AutoFixture;
using Bogus;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Cafe.Tests.Domain.UnitTests;

public class OrderItemTests
{
    private readonly Faker _faker = new();
    private readonly Faker<Product> _productFaker;
    private readonly Faker<OrderItem> _orderItemFaker;

    public OrderItemTests()
    {
        _productFaker = new Faker<Product>()
            .CustomInstantiator(f => Product.Create(
                f.Random.String(5, 20),
                f.Random.String(10, 80),
                new Money(f.Random.Decimal(1, 100), "RUB"),
                f.PickRandom<ProductCategory>()));

        _orderItemFaker = new Faker<OrderItem>()
            .CustomInstantiator(f =>
            {
                var product = _productFaker.Generate();
                var quantity = f.Random.Int(2, 10);
                return OrderItem.Create(product.Id, product.Name, product.Price, quantity);
            });
    }

    [Fact]
    public void Create_WithValidParameters_ShouldSucceed()
    {
        var product = _productFaker.Generate();
        var quantity = _faker.Random.Int(1, 10);

        var orderItem = OrderItem.Create(product.Id, product.Name, product.Price, quantity);

        orderItem.ProductId.Should().Be(product.Id);
        orderItem.ProductName.Should().Be(product.Name);
        orderItem.UnitPrice.Should().Be(product.Price);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.TotalPrice.Should().Be(product.Price * quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-4)]
    public void Create_WithInvalidQuantity_ShouldThrowException(int quantity)
    {
        var product = _productFaker.Generate();

        var action = () => OrderItem.Create(product.Id, product.Name, product.Price, quantity);

        action.Should().Throw<OrderingDomainException>().WithMessage("Количество должно быть больше 0");
    }

    [Fact]
    public void IncreaseQuantity_WithValidAmount_ShouldSucceed()
    {
        var orderItem = _orderItemFaker.Generate();
        var quantity = orderItem.Quantity;
        var increaseQuantity = _faker.Random.Int(1, 10);

        orderItem.IncreaseQuantity(increaseQuantity);

        orderItem.Quantity.Should().Be(quantity + increaseQuantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-4)]
    public void IncreaseQuantity_WithInvalidAmount_ShouldThrowException(int quantity)
    {
        var orderItem = _orderItemFaker.Generate();

        var action = () => orderItem.IncreaseQuantity(quantity);

        action.Should().Throw<OrderingDomainException>().WithMessage("Количество должно быть больше 0");
    }
    
    [Fact]
    public void DecreaseQuantity_WithValidAmount_ShouldSucceed()
    {
        var orderItem = _orderItemFaker.Generate();
        var quantity = orderItem.Quantity;
        var decreaseQuantity = _faker.Random.Int(1, orderItem.Quantity - 1);

        orderItem.DecreaseQuantity(decreaseQuantity);

        orderItem.Quantity.Should().Be(quantity - decreaseQuantity);
    }
    
    [Fact]
    public void DecreaseQuantity_ToZero_ShouldThrowException()
    {
        var orderItem = _orderItemFaker.Generate();
        var quantity = orderItem.Quantity;

        orderItem.DecreaseQuantity(quantity);

        orderItem.Quantity.Should().Be(0);
    }

    [Fact]
    public void DecreasePrice_WithTooLargeAmount_ShouldThrowException()
    {
        var orderItem = _orderItemFaker.Generate();
        var tooLargeQuantity = orderItem.Quantity + 1;
        
        var action = () => orderItem.DecreaseQuantity(tooLargeQuantity);
        
        action.Should().Throw<OrderingDomainException>().WithMessage("Нельзя вычитать количество большее, чем текущее");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-4)]
    public void DecreaseQuantity_WithInvalidAmount_ShouldThrowException(int quantity)
    {
        var orderItem = _orderItemFaker.Generate();
        
        var action = () => orderItem.DecreaseQuantity(quantity);

        action.Should().Throw<OrderingDomainException>().WithMessage("Количество должно быть больше 0");
    }
    
    
}