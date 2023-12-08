using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorLogic.DTOs.Product;

public record ProductWithOffersReadDTO
(
    Guid Id,
    string Name,
    decimal Price,
    ProductState State,
    List<OfferReadDTO> Offers
);