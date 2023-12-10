using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;
using OfferNegotiatorDal.Models;
using OfferNegotiatorDal.Repositories;
using OfferNegotiatorDal.Repositories.Interfaces;

namespace OfferNegotiatorTests.RepositoriesTests;

[TestFixture]
public class BaseRepositoryTests
{
    private IBaseRepository<Product> _baseRepository;
    private OfferNegotiatorContext _dbContext;
    private static readonly List<Product> _products = new()
        {
            new Product("TestProduct1", 100),
            new Product("TestProduct2", 200),
            new Product("TestProduct3", 300),
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
        await _dbContext.SaveChangesAsync();
        _baseRepository = new BaseRepository<Product>(_dbContext);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Test]
    public async Task GetByIdAsync_EntityExists_ReturnsEntity()
    {
        // Arrange
        var entityId = _products.First().Id;

        // Act
        var result = await _baseRepository.GetByIdAsync(entityId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(entityId));
        });
    }

    [Test]
    public async Task GetByIdAsync_EntityDoesNotExists_ReturnsNull()
    {
        // Arrange
        var entityId = Guid.NewGuid();

        // Act
        var result = await _baseRepository.GetByIdAsync(entityId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddAsync_AddEntity_ReturnsAddedEntity()
    {
        // Arrange
        var entityToAdd = new Product("TestProduct4", 400);

        // Act
        var result = await _baseRepository.AddAsync(entityToAdd);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(entityToAdd));
        });
    }

    [Test]
    public void AddAsync_AddNull_ThrowsArgumentNullException()
    {
        // Act
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _baseRepository.AddAsync(null));
    }

    [Test]
    public async Task UpdateAsync_UpdateEntity_ReturnsUpdatedEntity()
    {
        // Arrange
        var entityToUpdate = _products[0];
        entityToUpdate.Name = "TestName";
        entityToUpdate.Price = 1;
        entityToUpdate.State = OfferNegotiatorDal.Models.Enums.ProductState.Sold;

        // Act
        var result = await _baseRepository.UpdateAsync(entityToUpdate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(entityToUpdate));
        });
    }

    [Test]
    public void UpdateAsync_UpdateNull_ThrowsArgumentNullException()
    {
        // Act
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _baseRepository.UpdateAsync(null));
    }

    [Test]
    public async Task DeleteAsync_EntityExist_DeleteEntity()
    {
        // Arrange
        var entityToDelete = _products[1];

        // Act
        await _baseRepository.DeleteAsync(entityToDelete);
        var deletedEntity = await _dbContext.Products.FindAsync(entityToDelete.Id);

        // Assert
        Assert.That(deletedEntity, Is.Null);
    }

    [Test]
    public void DeleteAsync_DeleteNull_ThrowsArgumentNullException()
    {
        // Act
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _baseRepository.DeleteAsync(null));
    }
}