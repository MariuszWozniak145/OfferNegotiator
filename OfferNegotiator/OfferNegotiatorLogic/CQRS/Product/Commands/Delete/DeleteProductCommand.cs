using MediatR;

namespace OfferNegotiatorLogic.CQRS.Product.Commands.Delete;

public class DeleteProductCommand : IRequest
{
    public Guid ProductId { get; init; }

    public DeleteProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}