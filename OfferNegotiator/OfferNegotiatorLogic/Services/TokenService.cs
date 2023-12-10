using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfferNegotiatorLogic.Services.Interfaces;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using OfferNegotiatorDal.Exceptions;

namespace OfferNegotiatorLogic.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(IdentityUser user, IList<string> roles)
    {
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int expirationMinutes);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
        var token = CreateJwtToken(CreateClaims(user, roles), CreateSigningCredentials(), expiration);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new(_configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"], claims, expires: expiration, signingCredentials: credentials);

    private List<Claim> CreateClaims(IdentityUser user, IList<string> roles)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Name, user.Id),
            };
            if (roles.Count != 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            return claims;
        }
        catch (Exception ex)
        {
            throw new InternalIdentityServerException("Server failed - TokenService", new List<string>() { ex.Message });
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"])),
                SecurityAlgorithms.HmacSha256);
    }
}
