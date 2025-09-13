using Bogus;

namespace Cafe.Tests.Domain.UnitTests.FakeObjects;

public interface IFakeObjectDataGenerator<T> where T : class
{
    Faker<T> GetFaker();
}