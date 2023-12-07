namespace OfferNegotiatorLogic.DTOs.Offer;

public record OfferCreateDTO
(
    Guid ClientId,
    Guid ProductId,
    decimal Price
);