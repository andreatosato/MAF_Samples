var builder = DistributedApplication.CreateBuilder(args);

var weatherService = builder.AddProject<Projects.AspireA2AAgents_Weathers>("aspirea2aagents-weathers");

var tourismService = builder.AddProject<Projects.AspireA2AAgents_Tourisms>("aspirea2aagents-tourisms");

builder.AddProject<Projects.AspireA2AAgents_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(weatherService)
    .WaitFor(tourismService);

builder.Build().Run();
