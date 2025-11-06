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

app.MapA2A(tourism, path: "/",
    taskManager => app.MapWellKnownAgentCard(taskManager, "/"));

await app.RunAsync();