using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public class GreetingTools
{
    public GreetingTools()
    {
    }
    [McpServerTool, Description("Counts the number of words in the input message.")] 
    public int WordCount(string message)
    {
        return message.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}