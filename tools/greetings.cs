using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.Json;

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
        var response = new
        {
            content = new List<object>
            {
                new
                {
                    type = "text",
                    text = msg
                }
            }
        };

        string jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

        return jsonResponse;
    }
}
