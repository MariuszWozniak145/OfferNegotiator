using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;
using OfferNegotiatorLogic.Services.Interfaces;
using System.Security.Claims;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Post;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferWithProductReadDTO>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IOfferService _offerService;
    private readonly IValidator<OfferCreateDTO> _validator;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public CreateOfferCommandHandler
        (
        IOfferRepository offerRepository,
        IOfferService offerService,
        IValidator<OfferCreateDTO> validator,
        IMapper mapper,
        IConfiguration configuration
        )
    {
        _offerRepository = offerRepository;
        _offerService = offerService;
        _validator = validator;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<OfferWithProductReadDTO> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Offer);
        if (!validationResult.IsValid) throw new ValidationFailedException("Validation failed", validationResult.Errors.Select(error => error.ErrorMessage));
        var clientId = Guid.Parse(request.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
        var offer = new OfferNegotiatorDal.Models.Offer(clientId, request.Offer.ProductId, request.Offer.Price);
        if (!await _offerService.CheckIfClientCanAddOffer(offer)) throw new BadRequestException("Bad request", new List<string>() { $"Client with id: {offer.ClientId} has reached the maximum number of offers ({_configuration["MaxNumberOfOfferPerClientForProduct"]}) for product with id: {offer.ProductId}." });
        await _offerService.CheckOfferConditions(offer);
        var createdOffer = await _offerRepository.AddAsync(offer) ?? throw new InternalEntityServerException("Server failed", new List<string>() { "Offer has not been created." });
        return _mapper.Map<OfferWithProductReadDTO>(createdOffer);
    }
}