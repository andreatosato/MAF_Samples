var builder = DistributedApplication.CreateBuilder(args);

var weatherService = builder.AddProject<Projects.AspireA2AAgents_Weathers>("aspirea2aagents-weathers")
    .WithUrls((e) =>
    {
        e.Urls.Add(new() { Url = "/agenta2a/v1/card", DisplayText = "Weather Agent", Endpoint = e.GetEndpoint("https") });
    });

var tourismService = builder.AddProject<Projects.AspireA2AAgents_Tourisms>("aspirea2aagents-tourisms")
    .WithUrls((e) =>
    {
        e.Urls.Add(new() { Url = "/agenta2a/v1/card", DisplayText = "Tourism Agent", Endpoint = e.GetEndpoint("https") });
    });

builder.AddProject<Projects.AspireA2AAgents_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(weatherService)
    .WithReference(tourismService)
    .WaitFor(weatherService)
    .WaitFor(tourismService);

builder.Build().Run();
