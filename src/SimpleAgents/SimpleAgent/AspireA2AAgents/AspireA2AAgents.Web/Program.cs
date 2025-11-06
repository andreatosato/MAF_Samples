using A2A;
using AspireA2AAgents.Web;
using AspireA2AAgents.Web.Components;
using Microsoft.Agents.AI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AgentService>();
builder.Services.AddKeyedScoped<Task<AIAgent>>("WeatherAgent", (serviceProvider, o) =>
{
    var httpClient = new HttpClient()
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("services__aspirea2aagents-weathers__https__0") ?? Environment.GetEnvironmentVariable("services__aspirea2aagents-weathers__http__0") ?? "http://localhost:5142"),
        Timeout = TimeSpan.FromSeconds(60)
    };
    var agentCardResolver = new A2ACardResolver(httpClient!.BaseAddress!, httpClient, agentCardPath: "/agenta2a/v1/card");
    return agentCardResolver.GetAIAgentAsync();
});

builder.Services.AddKeyedScoped<Task<AIAgent>>("TourismAgent", (serviceProvider, o) =>
{
    var httpClient = new HttpClient()
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("services__aspirea2aagents-tourisms__https__0") ?? Environment.GetEnvironmentVariable("services__aspirea2aagents-tourisms__http__0") ?? "http://localhost:5161"),
        Timeout = TimeSpan.FromSeconds(60)
    };
    var agentCardResolver = new A2ACardResolver(httpClient!.BaseAddress!, httpClient, agentCardPath: "/agenta2a/v1/card");
    return agentCardResolver.GetAIAgentAsync();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
