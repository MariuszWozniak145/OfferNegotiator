using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Patch;

public class AcceptOfferCommand : IRequest<OfferReadDTO>
{
    public Guid OfferId { get; init; }

    public AcceptOfferCommand(Guid offerId)
    {
        OfferId = offerId;
    }
}