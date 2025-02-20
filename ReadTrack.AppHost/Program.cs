using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");

var db = sql.AddDatabase("sqldb", "ReadTrack");

var api = builder.AddProject<ReadTrack_API>("api")
    .WithReference(db)
    .WaitFor(db)    
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../ReadTrack.Web.Angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT", port: 4200)
    .WithExternalHttpEndpoints()    
    .PublishAsDockerFile();

builder.AddProject<ReadTrack_Web_Blazor>("blazor")
    .WithReference(api);

builder.Build().Run();
