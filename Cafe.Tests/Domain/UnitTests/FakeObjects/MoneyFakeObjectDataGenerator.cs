using Bogus;
using Cafe.Domain.ValueObjects;

namespace Cafe.Tests.Domain.UnitTests.FakeObjects;

public class MoneyFakeObjectDataGenerator : IFakeObjectDataGenerator<Money>
{
    public Faker<Money> GetFaker()
    {
        return new Faker<Money>().CustomInstantiator(f =>
            new Money(f.Random.Decimal(1, 100), "RUB"));
    }
}