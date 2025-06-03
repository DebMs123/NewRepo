using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public class GreetingTools
{
    [McpServerTool, Description("Count the number of words in a message.")]
    public string WordCount(string message)
    {
        var msg = message.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return $"The message contains {msg} words.";
    }
}