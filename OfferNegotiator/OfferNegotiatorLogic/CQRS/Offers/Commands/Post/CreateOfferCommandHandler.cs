﻿using AutoMapper;
using FluentValidation;
using MediatR;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorLogic.DTOs.Offer;
using OfferNegotiatorLogic.Services.Interfaces;
using System.Security.Claims;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Post;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferWithProductReadDTO>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IBusinessLogic _businessLogic;
    private readonly IValidator<OfferCreateDTO> _validator;
    private readonly IMapper _mapper;

    public CreateOfferCommandHandler
        (
        IOfferRepository offerRepository,
        IBusinessLogic businessLogic,
        IValidator<OfferCreateDTO> validator,
        IMapper mapper
        )
    {
        _offerRepository = offerRepository;
        _businessLogic = businessLogic;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<OfferWithProductReadDTO> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Offer);
        if (!validationResult.IsValid) throw new ValidationFailedException("Validation failed", validationResult.Errors.Select(error => error.ErrorMessage));
        var clientId = Guid.Parse(request.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
        var offer = new OfferNegotiatorDal.Models.Offer(clientId, request.Offer.ProductId, request.Offer.Price);
        await _businessLogic.CheckOfferConditions(offer);
        var createdOffer = await _offerRepository.AddAsync(offer) ?? throw new InternalEntityServerException("Server failed", new List<string>() { "Offer has not been created." });
        return _mapper.Map<OfferWithProductReadDTO>(createdOffer);
    }
}