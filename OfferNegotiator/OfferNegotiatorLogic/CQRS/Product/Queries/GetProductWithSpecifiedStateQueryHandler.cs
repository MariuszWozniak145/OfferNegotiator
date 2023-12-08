using AutoMapper;
using MediatR;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Queries;

public class GetProductWithSpecifiedStateQueryHandler : IRequestHandler<GetProductWithSpecifiedStateQuery, List<ProductReadDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductWithSpecifiedStateQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductReadDTO>> Handle(GetProductWithSpecifiedStateQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsWithSpecifiedStateAsync(request.State);
        return _mapper.Map<List<ProductReadDTO>>(products);
    }
}