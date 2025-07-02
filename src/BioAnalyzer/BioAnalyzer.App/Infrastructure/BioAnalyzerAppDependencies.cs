using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Services;

namespace BioAnalyzer.App.Infrastructure;

public static class BioAnalyzerAppDependencies
{
    public static IServiceCollection AddBioAnalyzerAppDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddHttpClient<IResearchApiClient, ResearchApiClient>(client =>
        {
            client.BaseAddress = new Uri("https+http://researchApi");
        });
        services.AddScoped<ISearchService, SearchService>();
        return services;
    }   
}