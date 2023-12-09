using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Repositories.Interfaces;

namespace OfferNegotiatorDal.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(OfferNegotiatorContext dbContext) : base(dbContext) { }

    public async Task<List<Offer>> GetClientOffersForProductAsync(Guid productId, Guid clientId)
    {
        return await _dbContext.Offers.Where(o => o.ProductId == productId && o.ClientId == clientId).ToListAsync();
    }

    public async Task<List<Offer>> GetOffersForClientAsync(Guid clientId)
    {
        return await _dbContext.Offers.Include(o => o.Product).Where(o => o.ClientId == clientId).ToListAsync();
    }
}