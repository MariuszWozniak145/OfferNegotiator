using Serilog;

namespace OfferNegotiatorApi.Configurations;

public static class HostBuilderExtension
{
    public static void AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, config) =>
        {
            config
                .WriteTo.Console()
                .MinimumLevel.Error();
        });
    }
}