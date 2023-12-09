using OfferNegotiatorDal.Models;

namespace OfferNegotiatorDal.Repositories.Interfaces;

public interface IOfferRepository : IBaseRepository<Offer>
{
    Task<List<Offer>> GetOffersForClientAsync(Guid clientId);
    Task<List<Offer>> GetClientOffersForProductAsync(Guid productId, Guid clientId);
    Task<Offer?> GetOfferWithProductAsync(Guid offerId);
}