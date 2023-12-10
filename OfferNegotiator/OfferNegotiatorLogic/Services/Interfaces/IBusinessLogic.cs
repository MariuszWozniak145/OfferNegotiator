using OfferNegotiatorDal.Models;

namespace OfferNegotiatorLogic.Services.Interfaces;

public interface IBusinessLogic
{
    Task CheckOfferConditions(Offer offer);
    Task<Offer> AcceptOffer(Offer offer);
    Task<Offer> RejectOffer(Offer offer);
}
