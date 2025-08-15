var builder = DistributedApplication.CreateBuilder(args);

//
// var sql = builder.AddSqlServer("bioAnalyzerSql")
//     .WithVolume("bioAnalyzerSqlData", "/var/opt/mssql");
//    //.WithLifetime(ContainerLifetime.Persistent);
//
// var db = sql
//     .AddDatabase("BioAnalyzer");

var researchApi = builder
    .AddProject<Projects.BioAnalyzer_Research_Api>("researchApi")
    //.WithHttpEndpoint(env: "RESEARCH_API_PORT")
   // .WaitFor(db)
   // .WithReference(db)
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