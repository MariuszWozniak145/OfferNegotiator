using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorDal.Repositories;

namespace OfferNegotiatorTests.RepositoriesTests;

[TestFixture]
public class OfferRepositoryTests
{
    private IOfferRepository _offerRepository;
    private OfferNegotiatorContext _dbContext;
    private static readonly List<Guid> _clientsId = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
        };
    private static readonly List<Product> _products = new()
        {
            new Product("TestProduct1", 100),
            new Product("TestProduct2", 200),
            new Product("TestProduct3", 300) { State = OfferNegotiatorDal.Models.Enums.ProductState.Sold },
        };
    private static readonly List<Offer> _offers = new()
        {
            new Offer(_clientsId[0], _products[0].Id, 110),
            new Offer(_clientsId[1], _products[0].Id, 90),
            new Offer(_clientsId[0], _products[1].Id, 190),
            new Offer(_clientsId[1], _products[1].Id, 210),
            new Offer(_clientsId[0], _products[2].Id, 310) { State=OfferNegotiatorDal.Models.Enums.OfferState.Accepted },
            new Offer(_clientsId[1], _products[2].Id, 290) { State=OfferNegotiatorDal.Models.Enums.OfferState.Rejected },
        };

    [OneTimeSetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<OfferNegotiatorContext>()
            .UseInMemoryDatabase(databaseName: "OfferNegotiatorTestDatabase")
            .Options;
        _dbContext = new OfferNegotiatorContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
        await _dbContext.Products.AddRangeAsync(_products);
        await _dbContext.Offers.AddRangeAsync(_offers);
        await _dbContext.SaveChangesAsync();
        _offerRepository = new OfferRepository(_dbContext);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Test]
    public async Task GetClientOffersForProductAsync_OffersForClientAndProductExists_ReturnsClientOffersForProduct()
    {
        // Arrange
        var clientId = _clientsId.First();
        var productId = _products.First().Id;

        // Act
        var result = await _offerRepository.GetClientOffersForProductAsync(productId, clientId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            foreach (var offer in result)
            {
                Assert.That(offer.ClientId, Is.EqualTo(clientId));
                Assert.That(offer.Product.Id, Is.EqualTo(productId));
            }
        });
    }

    [Test]
    public async Task GetClientOffersForProductAsync_OffersForClientDoesNotExists_ReturnsEmptyList()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var productId = _products.First().Id;

        // Act
        var result = await _offerRepository.GetClientOffersForProductAsync(productId, clientId);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetClientOffersForProductAsync_OffersForProductDoesNotExists_ReturnsEmptyList()
    {
        // Arrange
        var clientId = _clientsId.First();
        var productId = Guid.NewGuid();

        // Act
        var result = await _offerRepository.GetClientOffersForProductAsync(productId, clientId);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetOffersForClientAsync_OffersForClientExists_ReturnsClientOffers()
    {
        // Arrange
        var clientId = _clientsId.First();

        // Act
        var result = await _offerRepository.GetOffersForClientAsync(clientId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            foreach (var offer in result)
            {
                Assert.That(offer.ClientId, Is.EqualTo(clientId));
            }
        });
    }

    [Test]
    public async Task GetOffersForClientAsync_OffersForClientDoesNotExists_ReturnsEmptyList()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var result = await _offerRepository.GetOffersForClientAsync(clientId);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetOfferWithProductAsync_OfferExists_ReturnsOffer()
    {
        // Arrange
        var offerId = _offers.First().Id;
        var productId = _offers.First().ProductId;

        // Act
        var result = await _offerRepository.GetOfferWithProductAsync(offerId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(offerId));
            Assert.That(result.ProductId, Is.EqualTo(productId));
        });
    }

    [Test]
    public async Task GetOfferWithProductAsync_OfferDoesNotExists_ReturnsNul()
    {
        // Arrange
        var offerId = Guid.NewGuid();

        // Act
        var result = await _offerRepository.GetOfferWithProductAsync(offerId);

        // Assert
        Assert.That(result, Is.Null);
    }
}
