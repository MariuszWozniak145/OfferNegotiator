using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetOfferWithProductQuery : IRequest<OfferWithProductReadDTO>
{
    public Guid OfferId { get; init; }

    public GetOfferWithProductQuery(Guid offerId)
    {
        OfferId = offerId;
    }
}