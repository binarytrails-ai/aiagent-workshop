using AIAgent.API;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIAgent.API", Version = "v1" });
});

// Register AppConfig as a singleton
builder.Services.AddSingleton<AppConfig>(sp => new AppConfig(sp.GetRequiredService<IConfiguration>()));

var app = builder.Build();

// Initialize static Config wrapper for compatibility
Config.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
