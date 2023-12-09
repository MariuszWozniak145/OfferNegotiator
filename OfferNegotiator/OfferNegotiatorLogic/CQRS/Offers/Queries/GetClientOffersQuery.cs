using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetClientOffersQuery : IRequest<List<OfferWithProductReadDTO>>
{
    public Guid ClientId { get; init; }

    public GetClientOffersQuery(Guid clientId)
    {
        ClientId = clientId;
    }
}