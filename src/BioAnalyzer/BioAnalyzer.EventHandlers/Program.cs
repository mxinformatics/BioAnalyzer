using BioAnalyzer.EventHandlers.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.ConfigureFunctionsWebApplication();

_ = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton<EventHandlerConfiguration>((serviceProvider) => new EventHandlerConfiguration
{
    DownloadFileStorage = Environment.GetEnvironmentVariable("DownloadFileStorage") ??
        throw new InvalidOperationException("DownloadFileStorage environment variable is not set."),
    DownloadFileContainer = Environment.GetEnvironmentVariable("DownloadFileContainer") ??
        throw new InvalidOperationException("DownloadFileContainer environment variable is not set.")
});

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();