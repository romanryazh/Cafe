using Bogus;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;
using Cafe.Tests.Domain.UnitTests.FakeObjects;
using FluentAssertions;
using Xunit;

namespace Cafe.Tests.Domain.UnitTests;

public class OrderTests
{
    private readonly Faker _faker = new();
    private readonly IFakeObjectDataGenerator<Money> moneyGenerator;
    private readonly IFakeObjectDataGenerator<Product> productGenerator;
    private readonly IFakeObjectDataGenerator<OrderItem> orderItemGenerator;
    private readonly IFakeObjectDataGenerator<Order> orderGenerator;

    public OrderTests()
    {
        moneyGenerator = new MoneyFakeObjectDataGenerator();
        productGenerator = new ProductFakeObjectDataGenerator(moneyGenerator);
        orderItemGenerator = new OrderItemFakeObjectDataGenerator(moneyGenerator);
        orderGenerator = new OrderFakeObjectDataGenerator();
    }

    [Fact]
    public void Create_WithValidCustomerName_ShouldReturnOrder()
    {
        var guidOrder = _faker.Random.Guid();
        var customerName = "John Doe";

        var order = Order.Create(guidOrder, customerName);

        order.Id.Should().NotBeEmpty();
        order.Id.Should().Be(guidOrder);
        order.CustomerName.Should().Be(customerName);
        order.Status.Should().Be(OrderStatus.Created);
        order.OrderItems.Should().BeEmpty();
        order.TotalPrice.Amount.Should().Be(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidCustomerName_ShouldThrowException(string invalidCustomerName)
    {
        var action = () => Order.Create(_faker.Random.Guid(), invalidCustomerName);

        action.Should().Throw<OrderingDomainException>().WithMessage("Имя покупателя не может быть пустым");
    }

    [Fact]
    public void AddProduct_WithValidProduct_ShouldThrowException()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();
        var quantity = 2;

        order.AddItem(product, quantity);

        order.OrderItems.Should().HaveCount(1);
        var orderItem = order.OrderItems.Single();
        orderItem.ProductId.Should().Be(product.Id);
        orderItem.ProductName.Should().Be(product.Name);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.TotalPrice.Amount.Should().Be(product.Price.Amount * quantity);
    }

    [Fact]
    public void AddItem_WithSameProduct_ShouldIncreaseQuantity()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();

        order.AddItem(product, 2);
        order.AddItem(product, 5);

        order.OrderItems.Should().HaveCount(1);
        order.OrderItems.First().Quantity.Should().Be(7);
        order.TotalPrice.Amount.Should().Be(product.Price.Amount * 7);
    }

    [Fact]
    public void RemoveItem_WithValidQuantity_ShouldRemoveItem()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();

        order.AddItem(product, 5);
        order.RemoveItem(product, 2);

        order.OrderItems.Should().HaveCount(1);
        order.OrderItems.First().Quantity.Should().Be(3);
    }

    [Fact]
    public void RemoveItem_WithFullQuantity_ShouldRemoveItemCompletely()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();

        order.AddItem(product, 5);
        order.RemoveItem(product, 5);

        order.OrderItems.Should().BeEmpty();
        order.TotalPrice.Amount.Should().Be(0);
    }

    [Fact]
    public void MarkAsPaid_WithItems_ShouldChangeOrderStatus()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();
        order.AddItem(product, 1);
        
        order.MarkAsPaid();
        
        order.Status.Should().Be(OrderStatus.Paid);
    }

    [Fact]
    public void MarkAsPaid_WithEmptyOrder_ShouldThrowException()
    {
        var order = orderGenerator.GetFaker().Generate();
        
        var action = () => order.MarkAsPaid();
        
        action.Should().Throw<OrderingDomainException>().WithMessage("Нельзя оплатить пустой заказ");
    }

    [Fact]
    public void MarkAsCompleted_FromPaidStatus_ShouldChangeStatus()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();
        
        order.AddItem(product, 1);
        order.MarkAsPaid();
        
        order.MarkAsCompleted();
        
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void MarkAsCompleted_FromCreatedStatus_ShouldThrowException()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();
        order.AddItem(product, 1);
        
        var action = () => order.MarkAsCompleted();

        action.Should().Throw<OrderingDomainException>().WithMessage("Только оплаченный заказ может быть завершён");
    }

    [Fact]
    public void Cancel_FromCreatedStatus_ShouldChangeStatus()
    {
        var order = orderGenerator.GetFaker().Generate();
        
        order.Cancel();
        
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void Cancel_FromCompletedStatus_ShouldThrowException()
    {
        var order = orderGenerator.GetFaker().Generate();
        var product = productGenerator.GetFaker().Generate();
        
        order.AddItem(product, 1);
        order.MarkAsPaid();
        order.MarkAsCompleted();
        
        var action = () => order.Cancel();

        action.Should().Throw<OrderingDomainException>().WithMessage("Нельзя отменить завершённый или уже отменённый заказ");
    }
}