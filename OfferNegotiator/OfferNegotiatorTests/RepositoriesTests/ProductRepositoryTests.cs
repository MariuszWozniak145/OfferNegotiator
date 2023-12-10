using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Repositories.Interfaces;
using OfferNegotiatorDal.Repositories;
using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorTests.RepositoriesTests;

[TestFixture]
public class ProductRepositoryTests
{
    private IProductRepository _productRepository;
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
        };
    private static readonly List<Offer> _offers = new()
        {
            new Offer(_clientsId[0], _products[0].Id, 110),
            new Offer(_clientsId[1], _products[0].Id, 90),
            new Offer(_clientsId[0], _products[1].Id, 190),
            new Offer(_clientsId[1], _products[1].Id, 210),
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
        _productRepository = new ProductRepository(_dbContext);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Test]
    public async Task GetAllAsync_ProductsExist_ReturnsAllProducts()
    {
        // Act
        var result = await _productRepository.GetAllAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            foreach (var prodct in result)
            {
                Assert.That(prodct, Is.Not.Null);
            }
        });
    }

    [Test]
    public async Task GetProductWithOffersAsync_ProductWithOffersExist_ReturnsProductsWithOffers()
    {
        // Arrange
        var productId = _products.First().Id;

        // Act
        var result = await _productRepository.GetProductWithOffersAsync(productId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            foreach (var offer in result?.Offers)
            {
                Assert.That(offer, Is.Not.Null);
                Assert.That(offer?.ProductId, Is.EqualTo(productId));
            }
        });
    }

    [Test]
    public async Task GetProductWithOffersAsync_ProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var result = await _productRepository.GetProductWithOffersAsync(productId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetProductsWithSpecifiedStateAsync_ProductsExist_ReturnsProductsWithSpecifiedState()
    {
        // Arrange
        var requiredState = ProductState.Available;

        // Act
        var result = await _productRepository.GetProductsWithSpecifiedStateAsync(requiredState);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            foreach (var product in result)
            {
                Assert.That(product?.State, Is.EqualTo(requiredState));
            }
        });
    }

    [Test]
    public async Task GetProductsWithSpecifiedStateAsync_ProductsDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        var requiredState = ProductState.Sold;

        // Act
        var result = await _productRepository.GetProductsWithSpecifiedStateAsync(requiredState);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
