using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;

AIAgent agent = new AzureOpenAIClient(
  new Uri("https://legalserviceai.openai.azure.com/"),
  new AzureCliCredential())
    .GetChatClient("gpt-4.1-mini")
    .CreateAIAgent(instructions: "Tu sei bravo a raccontare barzellette!");

Console.WriteLine(await agent.RunAsync("Raccontami una barzelletta su un Pierino."));
Console.Read();