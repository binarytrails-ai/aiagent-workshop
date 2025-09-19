using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ContosoBikestore.MCPServer.Tools;

[McpServerToolType]
public sealed class BikeStoreTools
{
    private readonly HttpClient _client;
    private readonly ILogger<BikeStoreTools> _logger;
    private readonly string _baseUrl;

    public BikeStoreTools(HttpClient client, ILogger<BikeStoreTools> logger)
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
            _logger.LogInformation("[BikeStoreTools] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get bikes data: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[BikeStoreTools] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);

            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[BikeStoreTools] Exception occurred in GetAvailableBikes");
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
            _logger.LogInformation("[BikeStoreTools] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get bike with ID {bikeId}: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[BikeStoreTools] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);

            // Pretty format the JSON response
            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[BikeStoreTools] Exception occurred in GetBikeById");
            throw;
        }
    }

    [McpServerTool, Description("Create a new bike order.")]
    public async Task<string> CreateBikeOrder(
        [Description("The ID of the bike to order")] int bikeId,
        [Description("Customer's full name")] string customerName,
        [Description("Customer's email address")] string customerEmail,
        [Description("Shipping address for the order")] string shippingAddress)
    {
        try
        {
            var orderRequest = new
            {
                bikeId = bikeId,
                customerName = customerName,
                customerEmail = customerEmail,
                shippingAddress = shippingAddress
            };

            var jsonContent = JsonSerializer.Serialize(orderRequest);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var requestUri = $"{_baseUrl}/api/orders";

            using var response = await _client.PostAsync(requestUri, content);
            _logger.LogInformation("[BikeStoreTools] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to create order: {response.ReasonPhrase}";
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[BikeStoreTools] JSON Response: {JsonContent}", responseContent);
            using var jsonDocument = JsonDocument.Parse(responseContent);

            // Pretty format the JSON response
            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[BikeStoreTools] Exception occurred in CreateBikeOrder");
            throw;
        }
    }

    [McpServerTool, Description("Get order details by order ID.")]
    public async Task<string> GetOrderById(
        [Description("The order ID to retrieve")] string orderId)
    {
        try
        {
            var requestUri = $"{_baseUrl}/api/orders/{orderId}";
            using var response = await _client.GetAsync(requestUri);
            _logger.LogInformation("[BikeStoreTools] API Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            if (!response.IsSuccessStatusCode)
            {
                return $"Failed to get order with ID {orderId}: {response.ReasonPhrase}";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            _logger.LogTrace("[BikeStoreTools] JSON Response: {JsonContent}", jsonContent);
            using var jsonDocument = JsonDocument.Parse(jsonContent);

            return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[BikeStoreTools] Exception occurred in GetOrderById");
            throw;
        }
    }
}
