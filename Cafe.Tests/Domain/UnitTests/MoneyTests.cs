using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Cafe.Tests.Domain.UnitTests;

public class MoneyTests
{
    [Fact]
    public void CreateMoney_WithValidParameters_ShouldReturnMoney()
    {
        var amount = 100.00M;
        var currency = "RUB";
        
        var money = new Money(amount, currency);
        
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void CreateMoney_WithNegativeAmount_ShouldThrowException()
    {
        var amount = -100.00M;
        var currency = "RUB";
        
        Action act = () => new Money(amount, currency);
        
        act.Should().Throw<OrderingDomainException>().And.Message.Should().Contain("не может быть отрицательным");
    }

    [Fact]
    public void CreateMoney_WithEmptyCurrency_ShouldThrowException()
    {
        var amount = 100.00M;
        var currency = "";
        
        Action act = () => new Money(amount, currency);
        
        act.Should().Throw<OrderingDomainException>().And.Message.Should().Contain("не может быть пустой");
    }

    [Fact]
    public void AddMoney_WithValidAmount_ShouldAddMoney()
    {
        var money1 = new Money(100.00M, "RUB");
        var money2 = new Money(100.00M, "RUB");
        
        var result = money1 + money2;
        
        result.Amount.Should().Be(200.00M);
        result.Currency.Should().Be("RUB");
    }

    [Fact]
    public void AddMoney_WithDifferentCurrency_ShouldThrowException()
    {
        var money1 = new Money(100.00M, "RUB");
        var money2 = new Money(100.00M, "USD");
        
        Action act = () => money1 += money2;
        
        act.Should().Throw<OrderingDomainException>().And.Message.Should().Contain("Нельзя складывать деньги разной валюты");
    }

    [Fact]
    public void MultiplyMoney_ByQuantity_ShouldMultiplyMoney()
    {
        var money1 = new Money(100.00M, "RUB");
        var quantity = 3;
        
        var result = money1 * quantity;
        
        result.Amount.Should().Be(300.00M);
        result.Currency.Should().Be("RUB");
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        var money = new Money(100.00M, "RUB");
        
        var result = money.ToString();

        result.Should().Be("100,00 RUB");
    }
}