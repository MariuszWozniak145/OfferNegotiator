using FluentValidation;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.Validators.Product;

public class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductCreateValidator()
    {
        RuleFor(p => p.Name)
            .MaximumLength(255)
            .WithMessage("Name must not have more than 255 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}
