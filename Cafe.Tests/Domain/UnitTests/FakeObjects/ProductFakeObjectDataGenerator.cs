using Bogus;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.ValueObjects;
using FluentAssertions;

namespace Cafe.Tests.Domain.UnitTests.FakeObjects;

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