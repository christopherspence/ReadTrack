using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer("sql", password).WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("ReadTrack");

var api = builder
    .AddProject<ReadTrack_API>("api")
    .WaitFor(db)
    .WithReference(db);

var angular = builder.AddJavaScriptApp("angular", "../ReadTrack.Web.Angular")
    .WithRunScript("start")
    .WithReference(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

var mvc = builder.AddProject<ReadTrack_Web_Mvc>("mvc")
    .WaitFor(api)
    .WithReference(api);

builder.Build().Run();
