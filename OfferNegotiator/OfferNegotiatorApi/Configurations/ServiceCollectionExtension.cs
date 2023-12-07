using Microsoft.EntityFrameworkCore;
using OfferNegotiatorDal.DbContexts;

namespace OfferNegotiatorApi.Configurations;

public static class ServiceCollectionExtension
{
    public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OfferNegotiatorContext>(options =>
            options.UseInMemoryDatabase(databaseName: "OfferNegotiator"));
    }
}
