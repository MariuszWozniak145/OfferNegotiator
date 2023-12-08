using MediatR;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Queries;

public class GetProductsQuery : IRequest<List<ProductReadDTO>>
{
    public GetProductsQuery()
    {
    }
}