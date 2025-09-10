using Cafe.Domain.Exceptions;

namespace Cafe.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "RUB";

    public Money(decimal amount, string currency="RUB")
    {
        if (amount < 0)
            throw new OrderingDomainException("Значение не может быть отрицательным");

        if (string.IsNullOrWhiteSpace(currency))
            throw new OrderingDomainException("Валюта не может быть пустой");
        
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new OrderingDomainException("Нельзя складывать деньги разной валюты");
        
        return new Money(left.Amount + right.Amount, right.Currency);
    }

    public static Money operator *(Money left, int right)
    {
        return new Money(left.Amount * right, left.Currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}