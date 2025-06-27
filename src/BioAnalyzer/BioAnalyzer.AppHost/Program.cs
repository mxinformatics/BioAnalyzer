var builder = DistributedApplication.CreateBuilder(args);

var researchApi = builder
    .AddProject<Projects.BioAnalyzer_Research_Api>("researchApi")
    //.WithHttpEndpoint(env: "RESEARCH_API_PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();