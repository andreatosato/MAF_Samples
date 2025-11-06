using A2A;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AspireA2AAgents.Web;

public class AgentService
{
    private readonly IServiceProvider serviceProvider;

    public AgentService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public async Task<ChatClientAgent> GetResponses()
    {
        try
        {
            var weatherAgent = serviceProvider.GetRequiredKeyedService<Task<AIAgent>>("WeatherAgent");
            var tourismAgent = serviceProvider.GetRequiredKeyedService<Task<AIAgent>>("TourismAgent");

            // Connect to the remote agents via A2A
            var agents = await Task.WhenAll([weatherAgent, tourismAgent]);
            var tools = agents.Select(agent => (AITool)agent.AsAIFunction()).ToList();

            // Create the agent that uses the remote agents as tools
            var agent = new AzureOpenAIClient(new Uri("https://legalserviceai.openai.azure.com/"), new AzureCliCredential())
                .GetChatClient("gpt-4.1-mini")
                .CreateAIAgent(instructions: "Sei una guida turistica e dovrai dare al turista tutte le informazioni possibili che i tuoi tool riescono a recuperare.",
                    name: "HostAgent",
                    tools: tools);

            return agent;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
