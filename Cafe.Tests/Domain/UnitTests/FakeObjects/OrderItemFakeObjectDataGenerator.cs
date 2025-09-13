using Bogus;
using Cafe.Domain.Entities;
using Cafe.Domain.ValueObjects;

namespace Cafe.Tests.Domain.UnitTests.FakeObjects;

public class OrderItemFakeObjectDataGenerator(IFakeObjectDataGenerator<Money> moneyGenerator)  : IFakeObjectDataGenerator<OrderItem>
{
    public Faker<OrderItem> GetFaker()
    {
        var moneyFaker = moneyGenerator.GetFaker();
        
        return new Faker<OrderItem>().CustomInstantiator(f =>
            OrderItem.Create(
                f.Random.Guid(),
                f.Random.String(),
                moneyFaker.Generate(),
                f.Random.Int()));
    }
}