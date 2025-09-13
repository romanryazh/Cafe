using System.Collections.ObjectModel;
using Cafe.Domain.Enums;
using Cafe.Domain.Exceptions;
using Cafe.Domain.ValueObjects;

namespace Cafe.Domain.Entities;

public class Order : EntityBase
{
    private readonly List<OrderItem> _items = [];
    
    public string CustomerName { get; private set; }
    
    public OrderStatus Status { get; private set; }
    
    public Money TotalPrice { get; private set; } = new(0m, "RUB");
    
    public DateTime CreatedAt { get; private set; }
    
    public ReadOnlyCollection<OrderItem> OrderItems => _items.AsReadOnly();
    
    private Order() {}

    private Order(Guid id, string customerName)
    {
        Id = id;
        CustomerName = customerName;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public static Order Create(Guid id, string customerName)
    {
        ValidateCustomerName(customerName);
        
        return new Order(id, customerName);
    }

    public void AddItem(Product product, int quantity)
    {
        if (product == null)
            throw new OrderingDomainException("Продукт не может быть null");

        if (quantity <= 0)
            throw new OrderingDomainException("Количество должно быть больше 0");
        
        var existingItem = _items.FirstOrDefault(x => x.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var newItem = OrderItem.Create(product.Id, product.Name, product.Price, quantity);
            _items.Add(newItem);
        }
        
        RecalculateTotalPrice();
    }

    public void RemoveItem(Product product, int quantity)
    {
        if (quantity <= 0)
            throw new OrderingDomainException("Количество должно быть больше 0");
        
        var existingItem = _items.FirstOrDefault(x => x.ProductId == product.Id);
        if (existingItem == null)
            throw new OrderingDomainException("Предмет не найден в заказе");

        if (quantity >= existingItem.Quantity)
        {
            _items.Remove(existingItem);
        }
        else
        {
            existingItem.DecreaseQuantity(quantity);
        }
        
        RecalculateTotalPrice();
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new OrderingDomainException("Только созданный заказ может быть помечен как оплаченный");

        if (_items.Count == 0)
            throw new OrderingDomainException("Нельзя оплатить пустой заказ");
        
        Status = OrderStatus.Paid;
    }

    public void MarkAsCompleted()
    {
        if (Status != OrderStatus.Paid)
            throw new OrderingDomainException("Только оплаченный заказ может быть завершён");
        
        Status = OrderStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new OrderingDomainException("Нельзя отменить завершённый или уже отменённый заказ");
        
        Status = OrderStatus.Cancelled;
    }

    private void RecalculateTotalPrice()
    {
        TotalPrice = new Money(_items.Sum(i => i.TotalPrice.Amount), "RUB");
    }

    private static void ValidateCustomerName(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new OrderingDomainException("Имя покупателя не может быть пустым");

        if (customerName.Trim().Length > 50)
            throw new OrderingDomainException("Имя покупателя не может быть больше 50 символов");
    }
}