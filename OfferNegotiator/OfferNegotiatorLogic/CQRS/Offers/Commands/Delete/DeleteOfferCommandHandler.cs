using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Delete;

public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand>
{
    private readonly IOfferRepository _offerRepository;

    public DeleteOfferCommandHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetByIdAsync(request.OfferId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no offers with the given Id: {request.OfferId}." });
        await _offerRepository.DeleteAsync(offer);
    }
}