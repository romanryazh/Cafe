using Cafe.Domain.Enums;
using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;

namespace Cafe.Domain.Entities;

public class Product : EntityBase
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Money Price { get; private set; }
    
    public ProductCategory Category { get; private set; }

    private Product()
    {
    }

    private Product(string name, string description, Money price, ProductCategory category) : base(Guid.CreateVersion7())
    {
        Name = name;
        Description = description;
        Price = price;
        Category = category;
    }

    public static Product Create(string name, string description, Money price, ProductCategory category)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateCategory(category);
        
        return new Product(name.Trim(), description.Trim(), price, category);
    }

    public void Update(string name, string description, Money price, ProductCategory category)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateCategory(category);
        
        Name = name;
        Description = description;
        Price = price;
        Category = category;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new OrderingDomainException("Название продукта не может быть пустым");

        if (name.Length > 50)
            throw new OrderingDomainException("Название продукта не может быть длиннее 50 символов");
    }

    private static void ValidateDescription(string description)
    {
        if (description.Length > 100)
            throw new OrderingDomainException("Описание продукта не может быть длиннее 100 символов");
    }

    private static void ValidatePrice(Money price)
    {
        if (price == null) 
            throw new OrderingDomainException("Стоимость продукта не может быть null");
    }

    private static void ValidateCategory(ProductCategory category)
    {
        if (!Enum.IsDefined(category))
            throw new OrderingDomainException("Некорректная категория продукта");
    }
}
