using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetProductAsync(Guid productId);
    Task<Product?> GetProductWithOffersAsync(Guid productId);
    Task<List<Product>> GetProductsWithSpecifiedStateAsync(ProductState state);
}
