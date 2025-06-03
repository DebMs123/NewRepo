using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public class GreetingTools
{
    [McpServerTool, Description("Get a random Chuck Norris joke")]
    public string WordCount(string message)
    {
        var msg = message.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return $"The message contains {msg} words.";
    }
}