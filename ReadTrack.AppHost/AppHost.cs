using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer("sql", password).WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("db");

builder
    .AddProject<ReadTrack_API>("api")
    .WaitFor(db)
    .WithReference(db);

builder.Build().Run();
