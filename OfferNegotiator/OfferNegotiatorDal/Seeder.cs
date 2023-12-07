using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal.Models;

namespace OfferNegotiatorDal;

public static class Seeder
{
    public static readonly List<IdentityRole> Roles = new()
    {
        new IdentityRole("Employee"),
        new IdentityRole("Client")
    };

    public static readonly List<IdentityUser> Users = new()
    {
        new IdentityUser
        {
            UserName="Employee1",
            Id=Guid.NewGuid().ToString(),
            PasswordHash =  new PasswordHasher<IdentityUser>().HashPassword(null, "Password123")
        },
        new IdentityUser
        {
            UserName="Client1",
            Id=Guid.NewGuid().ToString(),
            PasswordHash =  new PasswordHasher<IdentityUser>().HashPassword(null, "Password123")
        },
        new IdentityUser
        {
            UserName="Client2",
            Id=Guid.NewGuid().ToString(),
            PasswordHash =  new PasswordHasher<IdentityUser>().HashPassword(null, "Password123")
        },
    };

    public static readonly List<IdentityUserRole<string>> UserRoles = new()
    {
        new IdentityUserRole<string>
        {
            RoleId = Roles[0].Id,
            UserId = Users[0].Id
        },
        new IdentityUserRole<string>
        {
            RoleId = Roles[1].Id,
            UserId = Users[1].Id
        },
        new IdentityUserRole<string>
        {
            RoleId = Roles[1].Id,
            UserId = Users[2].Id
        }
    };

    public static readonly List<Product> Products = new()
    {
        new Product("Product1", 100),
        new Product("Product2", 200),
        new Product("Product3", 300)
    };

    public static readonly List<Offer> Offers = new()
    {
        new Offer(Guid.Parse(Users[1].Id), Products[0].Id, 110),
        new Offer(Guid.Parse(Users[2].Id), Products[0].Id, 90),
        new Offer(Guid.Parse(Users[1].Id), Products[1].Id, 190),
        new Offer(Guid.Parse(Users[2].Id), Products[1].Id, 210),
        new Offer(Guid.Parse(Users[1].Id), Products[2].Id, 315),
        new Offer(Guid.Parse(Users[2].Id), Products[2].Id, 285)
    };
}
