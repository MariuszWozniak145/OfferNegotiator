using AutoMapper;
using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;
using OfferNegotiatorLogic.Services.Interfaces;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Patch;

public class RejectOfferCommandHandler : IRequestHandler<RejectOfferCommand, OfferReadDTO>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IBusinessLogic _businessLogic;
    private readonly IMapper _mapper;

    public RejectOfferCommandHandler
        (
        IOfferRepository offerRepository,
        IBusinessLogic businessLogic,
        IMapper mapper
        )
    {
        _offerRepository = offerRepository;
        _businessLogic = businessLogic;
        _mapper = mapper;
    }

    public async Task<OfferReadDTO> Handle(RejectOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetByIdAsync(request.OfferId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no offer with the given Id: {request.OfferId}." });
        var rejectedOffer = await _businessLogic.RejectOffer(offer);
        return _mapper.Map<OfferReadDTO>(rejectedOffer);
    }
}