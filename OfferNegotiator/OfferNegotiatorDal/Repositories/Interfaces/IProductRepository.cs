using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdWithOffersAsync(Guid id);
    Task<List<Product>> GetProductsWithSpecifiedStateAsync(ProductState state);
}
