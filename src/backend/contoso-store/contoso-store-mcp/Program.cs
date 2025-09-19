using ContosoBikestore.MCPServer.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry collection
builder.Services.AddApplicationInsightsTelemetry();

// Register HttpClient for DI
builder.Services.AddHttpClient();

builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Information);
builder.Services
    .AddMcpServer()
    //.WithStdioServerTransport()
    .WithHttpTransport()
    //.WithToolsFromAssembly()
    .WithTools<BikeStoreTools>();


var app = builder.Build();

// API Key Authentication Middleware
const string API_KEY_HEADER_NAME = "X-API-KEY";
var apiKey = builder.Configuration["ApiKey"] ?? Environment.GetEnvironmentVariable("API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    app.Logger.LogWarning("No API key configured. Set 'ApiKey' in appsettings.json or 'API_KEY' environment variable.");
}

app.Use(async (context, next) =>
{
    if (string.IsNullOrEmpty(apiKey))
    {
        // No API key configured, allow all requests (for local/dev)
        await next();
        return;
    }

    if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey) || extractedApiKey != apiKey)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized: Invalid or missing API Key.");
        return;
    }
    await next();
});

app.MapMcp();
app.Run();
