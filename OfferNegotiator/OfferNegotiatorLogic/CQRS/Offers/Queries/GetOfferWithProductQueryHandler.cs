using AutoMapper;
using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetOfferWithProductQueryHandler : IRequestHandler<GetOfferWithProductQuery, OfferWithProductReadDTO>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public GetOfferWithProductQueryHandler(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<OfferWithProductReadDTO> Handle(GetOfferWithProductQuery request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetOfferWithProductAsync(request.OfferId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no offer with the given Id: {request.OfferId}." });
        return _mapper.Map<OfferWithProductReadDTO>(offer);
    }
}