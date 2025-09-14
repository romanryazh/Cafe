using Bogus;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.ValueObjects;

namespace Cafe.Tests.FakeObjects;

public class ProductFakeObjectDataGenerator(IFakeObjectDataGenerator<Money> moneyGenerator) : IFakeObjectDataGenerator<Product>
{
    public Faker<Product> GetFaker()
    {
        var moneyFaker = moneyGenerator.GetFaker();
        
        return new Faker<Product>().CustomInstantiator(f =>
            Product.Create(
                f.Random.String(10),
                f.Random.String(30),
                moneyFaker.Generate(),
                f.PickRandom<ProductCategory>()));
    }
}