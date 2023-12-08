using MediatR;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Queries;

public class GetProductWithOffersQuery : IRequest<ProductWithOffersReadDTO>
{
    public Guid ProductId { get; init; }

    public GetProductWithOffersQuery(Guid productId)
    {
        ProductId = productId;
    }
}