using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.State)
           .IsRequired()
           .HasConversion(
                v => v.ToString(),
                v => (ProductState)Enum.Parse(typeof(ProductState), v));

        builder.HasMany(p => p.Offers)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId)
            .IsRequired();
    }
}