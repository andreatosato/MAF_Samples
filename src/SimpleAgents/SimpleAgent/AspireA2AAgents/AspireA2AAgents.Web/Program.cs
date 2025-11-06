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
builder.Services.AddHttpClient("weather", client => client.BaseAddress = new("https://aspirea2aagents-weathers"));
builder.Services.AddHttpClient("tourism", client => client.BaseAddress = new("https://aspirea2aagents-tourisms"));
builder.Services.AddKeyedScoped<Task<AIAgent>>("WeatherAgent", (serviceProvider, o) =>
{
    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
    var httpWeather = httpClientFactory!.CreateClient("weather");
    var agentCardResolver = new A2ACardResolver(httpWeather!.BaseAddress!);
    return agentCardResolver.GetAIAgentAsync();
});

builder.Services.AddKeyedScoped<Task<AIAgent>>("TourismAgent", (serviceProvider, o) =>
{
    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
    var httpTourism = httpClientFactory!.CreateClient("tourism");
    var agentCardResolver = new A2ACardResolver(httpTourism!.BaseAddress!);
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
