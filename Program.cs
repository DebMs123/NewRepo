using ModelContextProtocol;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using McpServer.Tools; // Add this if JsonRpcMessage is defined here
var builder = WebApplication.CreateBuilder(args);
// Register MCP server and discover tools from the current assembly
builder.Services.AddMcpServer().WithHttpTransport().WithTools<GreetingTools>();
// Add CORS for HTTP transport support in browsers
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable CORS
app.UseCors();
// Add MCP middleware
MapAbsoluteEndpointUriMcp(app);
// Add a simple home page
app.MapGet("/", () => "MCP Server on Azure App Service is running!");
app.Run();
static void MapAbsoluteEndpointUriMcp(IEndpointRouteBuilder endpoints)
{
    var loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();
    var options = endpoints.ServiceProvider.GetRequiredService<IOptions<McpServerOptions>>().Value;
    var routeGroup = endpoints.MapGroup("");
    SseResponseStreamTransport? session = null;

    routeGroup.MapGet("/sse", async context =>
    {
        context.Response.Headers.ContentType = "text/event-stream";

        var host = $"mcpwebappc-fgghguavh8c0e7dm.centralus-01.azurewebsites.net";
        var transport = new SseResponseStreamTransport(context.Response.Body, $"{host}/message");
        session = transport;
        try
        {
            await using (transport)
            {
                var transportTask = transport.RunAsync(context.RequestAborted);
                await using var server = McpServerFactory.Create(transport, options, loggerFactory, endpoints.ServiceProvider);

                try
                {
                    await server.RunAsync(context.RequestAborted);
                }
                catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
                {
                    // Normal SSE disconnect.
                }
                catch (Exception ex)
                {
                    // Handle other exceptions as needed.
                    var logger = loggerFactory.CreateLogger("SSETransport");
                    logger.LogError(ex, "Error in SSE transport: {Message}", ex.Message);
                }

                await transportTask;
            }
        }
        catch (Exception ex)
        {

        }
    });

    routeGroup.MapPost("/message", async context =>
    {
        if (session is null)
        {
            await Results.BadRequest("Session not started.").ExecuteAsync(context);
            return;
        }
        var message = await context.Request.ReadFromJsonAsync<JsonRpcMessage>(
            McpJsonUtilities.DefaultOptions, context.RequestAborted);
        if (message is null)
        {
            await Results.BadRequest("No message in request body.").ExecuteAsync(context);
            return;
        }

        await session.OnMessageReceivedAsync(message, context.RequestAborted);
        context.Response.StatusCode = StatusCodes.Status202Accepted;
        await context.Response.WriteAsync("Accepted");
    });
}
