using Microsoft.Extensions.Configuration;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.Services.Interfaces;

namespace OfferNegotiatorLogic.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IConfiguration _configuration;

    public OfferService
        (
        IOfferRepository offerRepository,
        IProductRepository productRepository,
        IConfiguration configuration
        )
    {
        _offerRepository = offerRepository;
        _productRepository = productRepository;
        _configuration = configuration;
    }

    public async Task<bool> CheckIfClientCanAddOffer(Offer offer)
    {
        var clientOffers = await _offerRepository.GetClientOffersForProductAsync(offer.ProductId, offer.ClientId);
        _ = int.TryParse(_configuration["MaxNumberOfOfferPerClientForProduct"], out int maxOffers);
        return clientOffers.Count < maxOffers;
    }

    public async Task CheckOfferConditions(Offer offer)
    {
        var product = await _productRepository.GetByIdAsync(offer.ProductId);
        _ = decimal.TryParse(_configuration["MaxPriceOverrun"], out decimal maxPriceOverrun);
        if (product.Price * maxPriceOverrun < offer.Price) offer.State = OfferNegotiatorDal.Models.Enums.OfferState.Rejected;
    }

    public Task AcceptOffer(Offer offer)
    {
        throw new NotImplementedException();
    }

    public Task RejectOffer(Offer offer)
    {
        throw new NotImplementedException();
    }
}
