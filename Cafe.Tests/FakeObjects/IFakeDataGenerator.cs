using Bogus;

namespace Cafe.Tests.FakeObjects;

public interface IFakeObjectDataGenerator<T> where T : class
{
    Faker<T> GetFaker();
}