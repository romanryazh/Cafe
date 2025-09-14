using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cafe.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(p => p.Price, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("price_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("price_currency")
                .HasMaxLength(3)
                .IsRequired();
        });
        
        builder.Property(p => p.Category)
            .HasConversion(
                vo => vo.ToString(),
                v => Enum.Parse<ProductCategory>(v))
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Category);
    }
}