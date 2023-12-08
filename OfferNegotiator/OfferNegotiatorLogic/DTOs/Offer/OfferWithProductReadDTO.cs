using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.DTOs.Offer;

public record OfferWithProductReadDTO
(
    Guid Id,
    Guid ClientId,
    Guid ProductId,
    ProductReadDTO Product,
    decimal Price,
    OfferState State
);