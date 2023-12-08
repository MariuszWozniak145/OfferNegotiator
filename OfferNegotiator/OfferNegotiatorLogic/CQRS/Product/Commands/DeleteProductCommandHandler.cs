using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;

namespace OfferNegotiatorLogic.CQRS.Product.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId) ?? throw new NotFoundException("Not found", new List<string>() { $"There is no product with the given Id: {request.ProductId}." });
        await _productRepository.DeleteAsync(product);
    }
}