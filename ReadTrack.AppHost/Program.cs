using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<ReadTrack_API>("api")
    .WithExternalHttpEndpoints();

builder.AddProject<ReadTrack_Web_Blazor>("blazor")
    .WithReference(api);

builder.AddNpmApp("angular", "../ReadTrack.Web.Angular")    
    .PublishAsDockerFile();

builder.Build().Run();
