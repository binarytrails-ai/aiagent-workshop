using ContosoBikestore.MCPServer.Tools;
using ContosoBikestore.MCPServer.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

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
    .WithTools<ProductInventoryTool>()
    .WithTools<OrderManagerTool>();

// Configure API Key header name from configuration or use default
string apiKeyHeaderName = builder.Configuration["ApiKey:HeaderName"] ?? "X-API-KEY";
builder.Services.AddAuthentication(builder =>
{
    builder.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
    builder.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
}).AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
    ApiKeyAuthenticationOptions.DefaultScheme,
    options => { options.ApiKeyHeaderName = apiKeyHeaderName; });

// Add authorization to require authentication for all endpoints
builder.Services.AddAuthorization(options =>
{
    // Create a specific policy for API key authentication
    options.AddPolicy("ApiKeyPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add(ApiKeyAuthenticationOptions.DefaultScheme);
        policy.RequireAuthenticatedUser();
    });
});


var app = builder.Build();

// API Key Authentication Middleware
//const string API_KEY_HEADER_NAME = "X-API-KEY";
//var apiKey = builder.Configuration["ApiKey"] ?? Environment.GetEnvironmentVariable("API_KEY");
//if (string.IsNullOrEmpty(apiKey))
//{
//    app.Logger.LogWarning("No API key configured. Set 'ApiKey' in appsettings.json or 'API_KEY' environment variable.");
//}

//app.Use(async (context, next) =>
//{
//    if (string.IsNullOrEmpty(apiKey))
//    {
//        // No API key configured, allow all requests (for local/dev)
//        await next();
//        return;
//    }

//    if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey) || extractedApiKey != apiKey)
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Unauthorized: Invalid or missing API Key.");
//        return;
//    }
//    await next();
//});

// Configure the HTTP request pipeline
app.UseAuthentication();
app.UseAuthorization();

// Map MCP endpoints with explicit API key authorization
app.MapMcp()
   .RequireAuthorization("ApiKeyPolicy"); // This ensures all MCP endpoints require API key authorization
app.Run();
