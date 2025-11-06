using A2A.AspNetCore;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var app = builder.Build();

AIAgent weatherAgent = new AzureOpenAIClient(
  new Uri("https://legalserviceai.openai.azure.com/"),
  new AzureCliCredential())
    .GetChatClient("gpt-4.1-mini")
    .CreateAIAgent(instructions:
    @"Sei un metereologo!
    Fornisci le previsioni meteo delle prossime ore",
    name: "WeatherAgent");

app.MapA2A(weatherAgent, 
    path: "/", 
    taskManager => app.MapWellKnownAgentCard(taskManager, "/"));

await app.RunAsync();