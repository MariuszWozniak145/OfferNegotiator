using FluentValidation;
using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.Validators.Offer;

public class OfferCreateValidator : AbstractValidator<OfferCreateDTO>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IProductRepository _productRepository;

    public OfferCreateValidator(UserManager<IdentityUser> userManager, IProductRepository productRepository)
    {
        _userManager = userManager;
        _productRepository = productRepository;

        RuleFor(o => o.ProductId)
            .MustAsync(CheckIfProductExistExist)
            .WithMessage("Product with this Id does not exist.");

        RuleFor(o => o.ClientId)
            .MustAsync(CheckIfUserExist)
            .WithMessage("Client with this Id does not exist.");

        RuleFor(o => o.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }

    private async Task<bool> CheckIfProductExistExist(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductAsync(id) != null;
    }

    private async Task<bool> CheckIfUserExist(Guid id, CancellationToken cancellationToken)
    {
        return await _userManager.FindByIdAsync(id.ToString()) != null;
    }
}
