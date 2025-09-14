using Bogus;
using Cafe.Domain.Entities;

namespace Cafe.Tests.FakeObjects;

public class OrderFakeObjectDataGenerator : IFakeObjectDataGenerator<Order>
{
    public Faker<Order> GetFaker()
    {
        return new Faker<Order>().CustomInstantiator(f =>
            Order.Create(
                f.Random.Guid(),
                f.Random.String(10)));
    }
}