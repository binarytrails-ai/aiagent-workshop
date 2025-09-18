using ContosoBikestore.MCPServer.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Information);
builder.Services
    .AddMcpServer()
    //.WithStdioServerTransport()
    .WithHttpTransport()
    //.WithToolsFromAssembly()
    .WithTools<BikeStoreTools>();

var app = builder.Build();
app.MapMcp();
app.Run();
