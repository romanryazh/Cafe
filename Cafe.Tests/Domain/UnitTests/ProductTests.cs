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

    private Product CreateProduct(string? name = null, string? description = null, Money? price = null,
        ProductCategory? category = null) => Product.Create(
        name ?? _fixture.Create<string>(),
        description ?? _fixture.Create<string>(),
        price ?? _fixture.Create<Money>(),
        category ?? _fixture.Create<ProductCategory>());

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
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateProduct_WithInvalidName_ShouldThrowException(string invalidName)
    {
        var action = () => CreateProduct(name: invalidName);

        action.Should().Throw<OrderingDomainException>().WithMessage("Название продукта не может быть пустым");
    }

    [Fact]
    public void CreateProduct_WithTooLongNameAndDescription_ShouldThrowException()
    {
        var longName = new string('*', 100);
        var longDescription = new string('*', 300);

        var action = () => CreateProduct(name: longName, description: longDescription);

        action.Should().Throw<OrderingDomainException>().And.Message.Should()
            .Contain("Название продукта не может быть длиннее");
    }

    [Fact]
    public void CreateProduct_WithInvalidProductCategory_ShouldThrowException()
    {
        var invalidProductCategory = (ProductCategory)999;

        var action = () => CreateProduct(category: invalidProductCategory);

        action.Should().Throw<OrderingDomainException>().And.Message.Should()
            .Contain("Некорректная категория продукта");
    }

    [Fact]
    public void CreateProduct_WithNullPrice_ShouldThrowException()
    {
        var action = () => Product.Create(
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            null!,
            _fixture.Create<ProductCategory>());

        action.Should().Throw<OrderingDomainException>().And.Message.Should()
            .Contain("Стоимость продукта не может быть null");
    }

    [Fact]
    public void UpdateProduct_WithValidData_ShouldSucceed()
    {
        var product = CreateProduct();
        var newName = _fixture.Create<string>();
        var newDescription = _fixture.Create<string>();
        var newPrice = _fixture.Create<Money>();
        var newCategory = _fixture.Create<ProductCategory>();
        
        product.Update(newName, newDescription, newPrice, newCategory);
        
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Should().Be(newPrice);
        product.Category.Should().Be(newCategory);
    }

}