using Microsoft.Extensions.Configuration;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.Services.Interfaces;

namespace OfferNegotiatorLogic.Services;

public class BusinessLogic : IBusinessLogic
{
    private readonly IOfferRepository _offerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IConfiguration _configuration;

    public BusinessLogic
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

    public async Task CheckOfferConditions(Offer offer)
    {
        await CheckIfProductIsAvailable(offer.ProductId);
        await CheckIfClientCanAddOffer(offer);
        await CheckOfferPrice(offer);
    }

    public async Task<Offer> AcceptOffer(Offer offer)
    {
        await CheckIfProductIsAvailable(offer.ProductId);
        CheckIfOfferIsNotSpecifiedState(offer, OfferState.Rejected);
        offer.State = OfferState.Accepted;
        var acceptedOffer = await _offerRepository.UpdateAsync(offer);
        var product = await _productRepository.GetProductWithOffersAsync(offer.ProductId);
        var offersToReject = product.Offers.Where(o => o.Id != offer.Id).ToList();
        await SetOffersStateToReject(offersToReject);
        product.State = ProductState.Sold;
        await _productRepository.UpdateAsync(product);
        return acceptedOffer;
    }

    public async Task<Offer> RejectOffer(Offer offer)
    {
        CheckIfOfferIsNotSpecifiedState(offer, OfferState.Accepted);
        offer.State = OfferState.Rejected;
        var rejectedOffer = await _offerRepository.UpdateAsync(offer);
        return rejectedOffer;
    }

    private async Task CheckIfProductIsAvailable(Guid productId)
    {
        var product = await _productRepository.GetProductWithOffersAsync(productId);
        if (product?.State != OfferNegotiatorDal.Models.Enums.ProductState.Available) throw new BadRequestException("Bad request", new List<string>() { $"Product with id: {productId} is not available." });
    }

    private void CheckIfOfferIsNotSpecifiedState(Offer offer, OfferState state)
    {
        if (offer?.State == state) throw new BadRequestException("Bad request", new List<string>() { $"Offer with id: {offer.Id} has an {state} status." });
    }

    private async Task SetOffersStateToReject(List<Offer> offers)
    {
        foreach (var offer in offers)
        {
            offer.State = OfferState.Rejected;
            await _offerRepository.UpdateAsync(offer);
        }
    }

    private async Task CheckIfClientCanAddOffer(Offer offer)
    {
        var clientOffers = await _offerRepository.GetClientOffersForProductAsync(offer.ProductId, offer.ClientId);
        _ = int.TryParse(_configuration["MaxNumberOfOfferPerClientForProduct"], out int maxOffers);
        if (clientOffers.Count >= maxOffers) throw new BadRequestException("Bad request", new List<string>() { $"Client with id: {offer.ClientId} has reached the maximum number of offers ({_configuration["MaxNumberOfOfferPerClientForProduct"]}) for product with id: {offer.ProductId}." });
    }

    private async Task CheckOfferPrice(Offer offer)
    {
        var product = await _productRepository.GetByIdAsync(offer.ProductId);
        _ = decimal.TryParse(_configuration["MaxPriceOverrun"], out decimal maxPriceOverrun);
        if (product.Price * maxPriceOverrun < offer.Price) offer.State = OfferNegotiatorDal.Models.Enums.OfferState.Rejected;
    }
}
