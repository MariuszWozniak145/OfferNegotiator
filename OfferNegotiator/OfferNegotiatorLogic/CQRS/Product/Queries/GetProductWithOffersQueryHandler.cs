using AutoMapper;
using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Queries;

public class GetProductWithOffersQueryHandler : IRequestHandler<GetProductWithOffersQuery, ProductWithOffersReadDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductWithOffersQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductWithOffersReadDTO> Handle(GetProductWithOffersQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductWithOffersAsync(request.ProductId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no product with the given Id: {request.ProductId}." });
        return _mapper.Map<ProductWithOffersReadDTO>(product);
    }
}