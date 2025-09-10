using AutoFixture;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Cafe.Tests.Domain.UnitTests;

public class ProductTests
{
    private readonly IFixture _fixture;

    public ProductTests()
    {
        _fixture = new Fixture();
        _fixture.Customize<Money>(c => c.FromFactory(() =>
            new Money(_fixture.Create<decimal>(), "RUB")));

        _fixture.Customize<Product>(c => c.FromFactory(() =>
            Product.Create(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                new Money(_fixture.Create<decimal>(), "RUB"),
                _fixture.Create<ProductCategory>())));
    }

    [Fact]
    public void CreateProduct_WithValidData_ShouldReturnProduct()
    {
        var name = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var category = _fixture.Create<ProductCategory>();
        var price = _fixture.Create<Money>();

        var product = Product.Create(name, description, price, category);

        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Category.Should().Be(category);
        product.Price.Should().Be(price);
        product.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateProduct_WithInvalidName_ShouldThrowException(string invalidName)
    {
        var action = () => Product.Create(
            invalidName,
            _fixture.Create<string>(),
            _fixture.Create<Money>(),
            _fixture.Create<ProductCategory>());

        action.Should().Throw<OrderingDomainException>().WithMessage("Название продукта не может быть пустым");
    }
}