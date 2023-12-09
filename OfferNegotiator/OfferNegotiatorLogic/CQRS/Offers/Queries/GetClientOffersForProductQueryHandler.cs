using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.CQRS.Offers.Queries;

public class GetClientOffersForProductQueryHandler : IRequestHandler<GetClientOffersForProductQuery, List<OfferReadDTO>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public GetClientOffersForProductQueryHandler(IOfferRepository offerRepository, IProductRepository productRepository, UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _productRepository = productRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<OfferReadDTO>> Handle(GetClientOffersForProductQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ClientId.ToString()) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no client with the given Id: {request.ClientId}." });
        var product = await _productRepository.GetByIdAsync(request.ProductId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no product with the given Id: {request.ProductId}." });
        var offers = await _offerRepository.GetClientOffersForProductAsync(request.ProductId, request.ClientId);
        return _mapper.Map<List<OfferReadDTO>>(offers);
    }
}