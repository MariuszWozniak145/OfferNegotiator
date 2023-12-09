using MediatR;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Delete;

public class DeleteOfferCommand : IRequest
{
    public Guid OfferId { get; init; }

    public DeleteOfferCommand(Guid offerId)
    {
        OfferId = offerId;
    }
}