using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ContosoBikestore.MCPServer.Tools;

[McpServerToolType]
public sealed class ProductInventoryTool
{
    private readonly HttpClient _client;
    private readonly ILogger<ProductInventoryTool> _logger;
    private readonly string _baseUrl;

    public ProductInventoryTool(HttpClient client, ILogger<ProductInventoryTool> logger)
    {
        _client = client;
        _logger = logger;
        _baseUrl = Environment.GetEnvironmentVariable("CONTOSO_STORE_URL") ??
            "https://aiagentwks-contoso-store-mbo43n.azurewebsites.net";
    }

    [McpServerTool, Description("Get all available bikes from the Contoso bike store.")]
    public async Task<string> GetAvailableBikes()
    {
        try
        {
            var requestUri = $"{_baseUrl}/api/bikes";
            using var response = await _client.GetAsync(requestUri);
            _logger.LogInformation("[ProductInventory] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get bikes data: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[ProductInventory] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);

            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductInventory] Exception occurred in GetAvailableBikes");
            throw;
        }
    }

    [McpServerTool, Description("Get details for a specific bike by its ID.")]
    public async Task<string> GetBikeById(
        [Description("The ID of the bike to retrieve")] int bikeId)
    {
        try
        {
            var requestUri = $"{_baseUrl}/api/bikes/{bikeId}";
            using var response = await _client.GetAsync(requestUri);
            _logger.LogInformation("[ProductInventory] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get bike with ID {bikeId}: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[ProductInventory] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);

            // Pretty format the JSON response
            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductInventory] Exception occurred in GetBikeById");
            throw;
        }
    }

    [McpServerTool, Description("Get bike ID by its name.")]
    public async Task<string> GetBikeIdByName(
        [Description("The name of the bike to find")] string bikeName)
    {
        try
        {
            // Get all bikes first
            var requestUri = $"{_baseUrl}/api/bikes";
            using var response = await _client.GetAsync(requestUri);
            _logger.LogInformation("[ProductInventory] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get bikes data: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[ProductInventory] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);
            
            // Find the bike with the matching name
            foreach (var bike in jsonDocument.RootElement.EnumerateArray())
            {
                if (bike.TryGetProperty("name", out var nameProperty) && 
                    nameProperty.GetString()?.Equals(bikeName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (bike.TryGetProperty("id", out var idProperty))
                    {
                        int id = idProperty.GetInt32();
                        return $"{{\"id\": {id}, \"name\": \"{bikeName}\"}}";
                    }
                }
            }
            
            return $"{{\"error\": \"No bike found with name '{bikeName}'\"}}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProductInventory] Exception occurred in GetBikeIdByName");
            throw;
        }
    }
}