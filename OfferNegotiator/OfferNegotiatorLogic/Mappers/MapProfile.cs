using AutoMapper;
using OfferNegotiatorDal.Models;
using OfferNegotiatorLogic.DTOs.Offer;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorLogic.Mappers;

internal class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Product, ProductReadDTO>();
        CreateMap<ProductCreateDTO, Product>();

        CreateMap<Offer, OfferReadDTO>();
        CreateMap<OfferCreateDTO, Offer>();
    }
}
