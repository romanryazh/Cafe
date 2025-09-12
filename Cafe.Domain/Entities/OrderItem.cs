using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;

namespace Cafe.Domain.Entities;

public class OrderItem : EntityBase
{
    public Guid ProductId { get; private set; }
    
    public string ProductName { get; private set; }
    
    public Money UnitPrice { get; private set; }
    
    public int Quantity { get; private set; }
    
    public Money TotalPrice => UnitPrice * Quantity;

    private OrderItem()
    {
    }

    private OrderItem(Guid productId, string productName, Money unitPrice, int quantity) : base(Guid.CreateVersion7())
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static OrderItem Create(Guid productId, string productName, Money unitPrice, int quantity)
    {
        ValidateQuantityLessThanZero(quantity);
        
        return new OrderItem(productId, productName, unitPrice, quantity);
    }

    public void IncreaseQuantity(int quantity)
    {
        ValidateQuantityLessThanZero(quantity);
        
        Quantity += quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        ValidateQuantityLessThanZero(quantity);

        if (quantity > Quantity)
            throw new OrderingDomainException("Нельзя вычитать количество большее, чем текущее");
        
        Quantity -= quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        ValidateQuantityLessThanZero(quantity);
        
        Quantity = quantity;
    }

    private static void ValidateQuantityLessThanZero(int quantity)
    {
        if (quantity <= 0)
            throw new OrderingDomainException("Количество должно быть больше 0");
    }
}