using MediatR;
using OfferNegotiatorLogic.DTOs.Offer;
using System.Security.Claims;

namespace OfferNegotiatorLogic.CQRS.Offers.Commands.Post;

public class CreateOfferCommand : IRequest<OfferWithProductReadDTO>
{
    public OfferCreateDTO Offer { get; init; }
    public IEnumerable<Claim> Claims { get; init; }

    public CreateOfferCommand(OfferCreateDTO offer, IEnumerable<Claim> claims)
    {
        Offer = offer;
        Claims = claims;
    }
}