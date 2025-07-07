var builder = DistributedApplication.CreateBuilder(args);

var researchApi = builder
    .AddProject<Projects.BioAnalyzer_Research_Api>("researchApi")
    //.WithHttpEndpoint(env: "RESEARCH_API_PORT")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.BioAnalyzer_App>("researchApp")
    .WithReference(researchApi)
    .WaitFor(researchApi)
    //.WithHttpEndpoint(env: "RESEARCH_APP_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddAzureFunctionsProject<Projects.BioAnalyzer_EventHandlers>("eventHandlers")
    .PublishAsDockerFile();

builder.Build().Run();