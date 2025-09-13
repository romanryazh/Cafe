using AutoFixture;
using Cafe.Domain.ValueObjects;

namespace Cafe.Tests.Domain.UnitTests;

public class MoneyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Money>(composer => composer.FromFactory(() =>
            new Money(fixture.Create<decimal>(), "RUB")));
    }
}