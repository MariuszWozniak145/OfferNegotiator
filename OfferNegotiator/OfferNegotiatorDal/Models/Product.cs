using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Models;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductState State { get; set; }
    public List<Offer> Offers { get; init; } = new();

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        State = ProductState.Available;
    }
}
