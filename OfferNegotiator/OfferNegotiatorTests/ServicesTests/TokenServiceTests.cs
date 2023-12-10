using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfferNegotiatorLogic.Services.Interfaces;
using OfferNegotiatorLogic.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using OfferNegotiatorDal.Exceptions;

namespace OfferNegotiatorTests.ServicesTests;

[TestFixture]
public class TokenServiceTests
{
    private ITokenService _tokenService;
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)

            .Build();
        _tokenService = new TokenService(_configuration);
    }

    [Test]
    public async Task CreateToken_ValidUserAndRoles_ReturnsToken()
    {
        // Arrange
        var user = new IdentityUser { Id = Guid.NewGuid().ToString() };
        var roles = new List<string> { "role1", "role2" };
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            ValidAudience = _configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"]))
        };

        // Act
        var token = _tokenService.CreateToken(user, roles);
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(principal, Is.Not.Null);
            Assert.That(principal.FindFirst(ClaimTypes.Name)?.Value, Is.EqualTo(user.Id));
            CollectionAssert.AreEquivalent(roles, principal.FindAll(ClaimTypes.Role).Select(c => c.Value));
        });
    }

    [Test]
    public void CreateToken_UserIsNull_ThrowsException()
    {
        // Arrange
        var roles = new List<string> { "role1", "role2" };

        // Act & Assert
        Assert.Throws<InternalIdentityServerException>(() => _tokenService.CreateToken(null, roles));
    }

    [Test]
    public void CreateToken_RolesIsNull_ThrowsException()
    {
        // Arrange
        var user = new IdentityUser { Id = Guid.NewGuid().ToString() };

        // Act & Assert
        Assert.Throws<InternalIdentityServerException>(() => _tokenService.CreateToken(user, null));
    }
}