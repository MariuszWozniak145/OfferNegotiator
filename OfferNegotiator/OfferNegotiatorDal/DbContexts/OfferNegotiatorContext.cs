using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.Models;
using System.Reflection;

namespace OfferNegotiatorDal.DbContexts;

public class OfferNegotiatorContext : DbContext
{
    public OfferNegotiatorContext(DbContextOptions<OfferNegotiatorContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Offer> Offers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
