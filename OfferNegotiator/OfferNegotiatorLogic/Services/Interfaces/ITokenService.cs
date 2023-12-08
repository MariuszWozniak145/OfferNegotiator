using Microsoft.AspNetCore.Identity;

namespace OfferNegotiatorLogic.Services.Interfaces;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, IList<string> roles);
}