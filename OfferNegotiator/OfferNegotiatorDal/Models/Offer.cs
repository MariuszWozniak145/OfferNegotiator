using OfferNegotiatorDal.Models.Enums;

namespace OfferNegotiatorDal.Models;

public class Offer
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid ProductId { get; init; }
    public Product Product { get; init; }
    public decimal Price { get; init; }
    public OfferState State { get; set; }

    public Offer(Guid clientId, Guid productId, decimal price)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
        ProductId = productId;
        Price = price;
        State = OfferState.Pending;
    }
}
