namespace OfferNegotiatorLogic.DTOs.Offer;

public record OfferCreateDTO
(
    Guid ProductId,
    decimal Price
);