using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(o => o.CustomerName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(o => o.Status)
            .HasConversion(
                vo => vo.ToString(),
                v => Enum.Parse<OrderStatus>(v))
            .HasMaxLength(20)
            .IsRequired();

        builder.OwnsOne(o => o.TotalPrice, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("total_price_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("total_price_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.OwnsMany(o => o.OrderItems, itemBuilder =>
        {
            itemBuilder.ToTable("order_items");
            itemBuilder.WithOwner().HasForeignKey("order_id");

            itemBuilder.Property(o => o.Id)
                .ValueGeneratedNever()
                .IsRequired();

            itemBuilder.Property(i => i.ProductId)
                .IsRequired();

            itemBuilder.Property(i => i.ProductName)
                .HasMaxLength(50)
                .IsRequired();

            itemBuilder.OwnsOne(i => i.UnitPrice, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount)
                    .HasColumnName("unit_price_amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                
                moneyBuilder.Property(m => m.Currency)
                    .HasColumnName("unit_price_currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            itemBuilder.Property(i => i.Quantity)
                .IsRequired();
            
            itemBuilder.HasKey("Id", "OrderId");
        });
        
        builder.Navigation(o => o.OrderItems).Metadata.SetField("_items");
        builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(o => o.CustomerName);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedAt);
    }
}