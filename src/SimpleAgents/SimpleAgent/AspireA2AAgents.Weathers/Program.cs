using A2A;
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
    path: "/agenta2a", 
    new AgentCard 
    {
        Name = "Weather Agent",
        Url = "http://localhost:5142/agenta2a",
        Description = "An agent that provides weather forecasts.",
        Version = "1.0.0",
        DefaultInputModes = ["text"],
        DefaultOutputModes = ["text"],
        Capabilities = new AgentCapabilities
        {
            Streaming = false,
            PushNotifications = false
        },
        Skills =
        {
            new AgentSkill
            {
                Name = "Get Weather Forecast",
                Description = "Provides weather forecasts for the next hours.",
                Examples = ["What's the weather like today?", "Will it rain tomorrow?" ]
            }
        }
    });

await app.RunAsync();