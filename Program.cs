using McpServer.Tools;
using ModelContextProtocol;
using ModelContextProtocol.Server;
var builder = WebApplication.CreateBuilder(args);
// Register MCP server and discover tools from the current assembly
builder.Services.AddMcpServer().WithHttpTransport().WithToolsFromAssembly();
var app = builder.Build();
// Add MCP middleware
app.MapMcp();
// Ensure routing is configured
app.UseRouting();
// Add a simple home page
app.MapGet("/home", () => "MCP Server on Azure App Service is running!");
app.Run();