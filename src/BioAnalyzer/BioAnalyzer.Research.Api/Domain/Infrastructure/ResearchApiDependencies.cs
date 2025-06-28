using BioAnalyzer.Research.Api.Domain.Clients;
using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Domain.Services;

namespace BioAnalyzer.Research.Api.Domain.Infrastructure;

public static class ResearchApiDependencies
{
    public static IServiceCollection AddResearchApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ResearchApiConfiguration>(configuration.GetSection("ResearchApi"))
            .PostConfigure<ResearchApiConfiguration>(config =>
            {
                config.ThrowIfInvalid();
            });

        services.AddHttpClient<IEntrezClient, EntrezClient>();

        services.AddHttpClient<INcbiClient, NcbiClient>();
        
        services.AddScoped<ILiteratureSearchService, LiteratureSearchService>();
        return services;
    }
}