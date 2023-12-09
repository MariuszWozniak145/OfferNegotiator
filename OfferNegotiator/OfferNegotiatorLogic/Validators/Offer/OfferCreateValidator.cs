using FluentValidation;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.Validators.Offer;

public class OfferCreateValidator : AbstractValidator<OfferCreateDTO>
{
    private readonly IProductRepository _productRepository;

    public OfferCreateValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(o => o.ProductId)
            .MustAsync(CheckIfProductExistExist)
            .WithMessage("Product with this Id does not exist.");

        RuleFor(o => o.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }

    private async Task<bool> CheckIfProductExistExist(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(id) != null;
    }
}
