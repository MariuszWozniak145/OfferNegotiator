using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.DTOs.Offer;

public record OfferWithProductReadDTO
(
    Guid Id,
    Guid ClientId,
    ProductReadDTO Product,
    decimal Price,
    OfferState State
);