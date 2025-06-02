using System.ComponentModel;
using MCP.HTTP.Server.Entities;
using MCP.HTTP.Server.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public class GreetingTools
{
    public GreetingTools()
    {
    }
    [McpServerTool, Description("Counts the number of words in the input message.")] 
    public async Task<int?> WordCount([Description("The input")] string message)
    {
        return await message.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
