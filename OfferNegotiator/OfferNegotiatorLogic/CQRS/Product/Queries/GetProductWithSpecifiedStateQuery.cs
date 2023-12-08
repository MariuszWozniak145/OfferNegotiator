using MediatR;
using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Queries;

public class GetProductWithSpecifiedStateQuery : IRequest<List<ProductReadDTO>>
{
    public ProductState State { get; init; }

    public GetProductWithSpecifiedStateQuery(ProductState state)
    {
        State = state;
    }
}