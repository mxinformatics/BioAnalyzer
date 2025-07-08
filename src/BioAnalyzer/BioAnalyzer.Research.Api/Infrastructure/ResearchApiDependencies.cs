using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BioAnalyzer.AzureStorage.Contexts;
using BioAnalyzer.AzureStorage.Contracts;
using BioAnalyzer.Research.Api.Domain.Clients;
using BioAnalyzer.Research.Api.Domain.Services;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Infrastructure;

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

        services.Configure<ResearchApiStorageConfiguration>(configuration.GetSection("ResearchStorage"))
            .PostConfigure<ResearchApiStorageConfiguration>(config =>
            {
                config.ThrowIfInvalid();
            });
        
        services.AddScoped<ITableContext, AzureTableContext>((provider) =>
        {
            var storageConfig = provider.GetRequiredService<IOptions<ResearchApiStorageConfiguration>>().Value;
            return new AzureTableContext(storageConfig);
        });

        services.AddScoped<IBlobContext, AzureBlobContext>((provider) =>
        {
            var storageConfig = provider.GetRequiredService<IOptions<ResearchApiStorageConfiguration>>().Value;
            return new AzureBlobContext(storageConfig);
        });
        services.AddScoped<IStorageClient, StorageClient>();
        
        services.AddScoped<ILiteratureSearchService, LiteratureSearchService>();
        services.AddScoped<ILiteratureService, LiteratureService>();
        return services;
    }
    
    public static WebApplicationBuilder AddSecureConfiguration(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var secureConfiguration = builder.Configuration.GetSection("SecureConfiguration").Get<SecureConfiguration>();
        if(secureConfiguration == null)
        {
            throw new InvalidOperationException("SecureConfiguration is required");
        }
        secureConfiguration.ThrowIfInvalid();

        if (secureConfiguration.UseKeyVault)
        {
            var secretClient = new SecretClient(
                new Uri(secureConfiguration.KeyVaultUrl),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        ExcludeVisualStudioCredential = true,
                        ExcludeEnvironmentCredential = secureConfiguration.ExcludeEnvironmentCredential
                    }
                ));

            builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());    
        }
        return builder;
    }
}