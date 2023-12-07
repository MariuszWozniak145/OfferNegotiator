using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal;
using OfferNegotiatorDal.DbContexts;

namespace OfferNegotiatorApi.Configurations;

public static class WebApplicationExtension
{
    public static async Task SeedOfferNegotiatorDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<OfferNegotiatorContext>();
        await dbContext.Products.AddRangeAsync(Seeder.Products);
        await dbContext.Offers.AddRangeAsync(Seeder.Offers);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedUserDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<UsersContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var user in Seeder.Users)
        {
            await userManager.CreateAsync(user);
        }
        foreach (var role in Seeder.Roles)
        {
            await roleManager.CreateAsync(role);
        }
        foreach (var userRole in Seeder.UserRoles)
        {
            var user = await userManager.FindByIdAsync(userRole.UserId);
            var role = await roleManager.FindByIdAsync(userRole.RoleId);
            await userManager.AddToRoleAsync(user, role.Name);
        }
    }
}
