using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BioAnalyzer.Research.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddSecureConfiguration(builder.Configuration);
// var secureConfiguration = builder.Configuration.GetSection("SecureConfiguration").Get<SecureConfiguration>();
// if(secureConfiguration == null)
// {
//     throw new InvalidOperationException("SecureConfiguration is required");
// }
// secureConfiguration.ThrowIfInvalid();
//
// if (secureConfiguration.UseKeyVault)
// {
//     var secretClient = new SecretClient(
//         new Uri(secureConfiguration.KeyVaultUrl),
//         new DefaultAzureCredential(
//             new DefaultAzureCredentialOptions
//             {
//                 ExcludeVisualStudioCredential = true,
//                 ExcludeEnvironmentCredential = secureConfiguration.ExcludeEnvironmentCredential
//             }
//         ));
//
//     builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());    
// }
builder.Services.AddResearchApiDependencies(builder.Configuration); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();