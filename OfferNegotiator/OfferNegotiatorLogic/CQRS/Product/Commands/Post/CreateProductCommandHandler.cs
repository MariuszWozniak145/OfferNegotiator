using AutoMapper;
using FluentValidation;
using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.CQRS.Product.Commands.Post;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductWithOffersReadDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ProductCreateDTO> _validator;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository productRepository, IValidator<ProductCreateDTO> validator, IMapper mapper)
    {
        _productRepository = productRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<ProductWithOffersReadDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Product);
        if (!validationResult.IsValid) throw new ValidationFailedException("Validation failed", validationResult.Errors.Select(error => error.ErrorMessage));
        var product = _mapper.Map<OfferNegotiatorDal.Models.Product>(request.Product);
        var createdProduct = await _productRepository.AddAsync(product) ?? throw new InternalEntityServerException("Server failed", new List<string>() { "Product has not been created." });
        return _mapper.Map<ProductWithOffersReadDTO>(createdProduct);
    }
}