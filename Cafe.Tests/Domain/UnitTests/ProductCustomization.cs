using AutoFixture;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.ValueObjects;

namespace Cafe.Tests.Domain.UnitTests;

public class ProductCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Product>(composer => composer.FromFactory(() => 
            Product.Create(
                fixture.Create<string>(),
                fixture.Create<string>(),
                new Money(fixture.Create<decimal>(), "RUB"),
                fixture.Create<ProductCategory>())));
    }
}