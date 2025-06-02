using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpServer.Tools;

[McpServerToolType]
public sealed class GreetingTools
{
    public GreetingTools()
    {
    }
    [McpServerTool, Description("Counts the number of words in the input message.")]
    public static int WordCount(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return 0;

        var msg = message.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return content: [
        {
          type: "text",
          text: msg,
        }
    }
}
