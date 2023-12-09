using OfferNegotiatorDal.Models;

namespace OfferNegotiatorLogic.Services.Interfaces;

public interface IOfferService
{
    Task<bool> CheckIfClientCanAddOffer(Offer offer);
    Task CheckOfferConditions(Offer offer);
    Task AcceptOffer(Offer offer);
    Task RejectOffer(Offer offer);
}
