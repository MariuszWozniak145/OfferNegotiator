using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorLogic.DTOs.Offer;

public record OfferReadDTO
(
    Guid Id,
    Guid ClientId,
    Guid ProductId,
    decimal Price,
    OfferState State
);