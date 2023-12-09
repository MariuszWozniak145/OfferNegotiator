using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetClientOffersForProductQuery : IRequest<List<OfferReadDTO>>
{
    public Guid ProductId { get; init; }
    public Guid ClientId { get; init; }

    public GetClientOffersForProductQuery(Guid productId, Guid clientId)
    {
        ProductId = productId;
        ClientId = clientId;
    }
}