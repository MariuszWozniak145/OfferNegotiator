using MediatR;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Commands.Post;

public class CreateProductCommand : IRequest<ProductReadDTO>
{
    public ProductCreateDTO Product { get; init; }

    public CreateProductCommand(ProductCreateDTO product)
    {
        Product = product;
    }
}