using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetClientOffersQueryHandler : IRequestHandler<GetClientOffersQuery, List<OfferWithProductReadDTO>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public GetClientOffersQueryHandler(IOfferRepository offerRepository, UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<OfferWithProductReadDTO>> Handle(GetClientOffersQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ClientId.ToString()) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no client with the given Id: {request.ClientId}." });
        var offers = await _offerRepository.GetOffersForClientAsync(request.ClientId);
        return _mapper.Map<List<OfferWithProductReadDTO>>(offers);
    }
}