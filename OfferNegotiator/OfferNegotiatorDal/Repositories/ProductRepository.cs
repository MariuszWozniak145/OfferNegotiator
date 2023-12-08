using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorDal.Repositories.Interfaces;

namespace OfferNegotiatorDal.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(OfferNegotiatorContext dbContext) : base(dbContext) { }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductAsync(Guid productId)
    {
        return await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product?> GetProductWithOffersAsync(Guid productId)
    {
        return await _dbContext.Products.Include(p => p.Offers).SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<List<Product>> GetProductsWithSpecifiedStateAsync(ProductState state)
    {
        return await _dbContext.Products.Where(p => p.State == state).ToListAsync();
    }
}
