using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Patch;

public class RejectOfferCommand : IRequest<OfferReadDTO>
{
    public Guid OfferId { get; init; }

    public RejectOfferCommand(Guid offerId)
    {
        OfferId = offerId;
    }
}