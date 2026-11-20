var builder = DistributedApplication.CreateBuilder(args);


var sqlPassword = builder.AddParameter("sql-parameter", secret: true);


var sql = builder.AddSqlServer("sql",sqlPassword)
	.WithDataVolume("aspire-demo-data2")  // Persistent storage — survives container restarts
	.WithEnvironment("ACCEPT_EULA", "Y")  // Accept SQL Server
	.WithEnvironment("MSSQL_PID", "Developer")  // Use free Developer edition
	.WithLifetime(ContainerLifetime.Persistent)  // Container should continue to run until manually stopped
	.WithEndpoint("tcp", e => e.Port = 54321);  // Expose SQL Server on localhost:44444

var db = sql.AddDatabase("demodb");

var apiService = builder.AddProject<Projects.AspireIntroDemo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.AspireIntroDemo_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
