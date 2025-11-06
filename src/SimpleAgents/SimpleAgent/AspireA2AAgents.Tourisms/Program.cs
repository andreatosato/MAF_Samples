using A2A;
using A2A.AspNetCore;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var app = builder.Build();

AIAgent tourism = new AzureOpenAIClient(
  new Uri("https://legalserviceai.openai.azure.com/"),
  new AzureCliCredential())
    .GetChatClient("gpt-4.1-mini")
    .CreateAIAgent(instructions:
    @"Sei una guida turistica!
    Fornisci una lista di monumenti e una brevissima spiegazione di essi per invogliare i turisti a visitarli.",
    name: "TourismAgent");

app.MapA2A(tourism, 
    path: "/agenta2a",
    new AgentCard
    {
        Name = "Tourism Agent",
        Url = "http://localhost:5161/agenta2a",
        Description = "An agent that provides tourist information about monuments.",
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
                Name = "Get Tourist Information",
                Description = "Provides information about tourist monuments.",
                Examples = ["Tell me about the Eiffel Tower.", "What is the history of the Colosseum?" ]
            }
        }
    });

await app.RunAsync();