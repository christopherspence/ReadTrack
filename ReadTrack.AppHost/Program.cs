using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");

var db = sql.AddDatabase("sqldb", "ReadTrack");

var api = builder.AddProject<ReadTrack_API>("api")
    .WithReference(db)
    .WaitFor(db)
    .WithExternalHttpEndpoints();

builder.AddProject<ReadTrack_Web_Blazor>("blazor")
    .WithReference(api);

builder.AddNpmApp("angular", "../ReadTrack.Web.Angular")    
    .PublishAsDockerFile();

builder.Build().Run();
