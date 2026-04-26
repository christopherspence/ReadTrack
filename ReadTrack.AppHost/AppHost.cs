using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer("sql", password).WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("db");

var api = builder
    .AddProject<ReadTrack_API>("api")
    .WaitFor(db)
    .WithReference(db);

builder.AddJavaScriptApp("angular", "../ReadTrack.Web.Angular")
    .WithRunScript("start")
    .WithReference(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.AddProject<ReadTrack_Web_Blazor>("blazor")
    .WaitFor(api)
    .WithReference(api);

builder.Build().Run();
