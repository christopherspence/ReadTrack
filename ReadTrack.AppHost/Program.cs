using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ReadTrack_API>("api")
    .WithExternalHttpEndpoints();

builder.AddProject<ReadTrack_Web_Blazor>("blazor");

builder.Build().Run();
