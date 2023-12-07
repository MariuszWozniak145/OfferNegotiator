using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.ClientId)
            .IsRequired();

        builder.Property(o => o.ProductId)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.State)
           .IsRequired()
           .HasConversion(
                v => v.ToString(),
                v => (OfferState)Enum.Parse(typeof(OfferState), v));
    }
}