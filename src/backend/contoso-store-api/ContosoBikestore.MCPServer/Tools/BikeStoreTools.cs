using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ContosoBikestore.MCPServer.Tools;

[McpServerToolType]
public sealed class BikeStoreTools
{
    private const string BaseUrl = "https://aiagentwks-contoso-store-mbo43n.azurewebsites.net";

    [McpServerTool, Description("Get all available bikes from the Contoso bike store.")]
    public static async Task<string> GetAvailableBikes(HttpClient client)
    {
        var requestUri = $"{BaseUrl}/api/bikes";

        using var response = await client.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            return $"Failed to get bikes data: {response.ReasonPhrase}";
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(jsonContent);

        // Pretty format the JSON response
        return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    [McpServerTool, Description("Get details for a specific bike by its ID.")]
    public static async Task<string> GetBikeById(
        HttpClient client,
        [Description("The ID of the bike to retrieve")] int bikeId)
    {
        var requestUri = $"{BaseUrl}/api/bikes/{bikeId}";

        using var response = await client.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            return $"Failed to get bike with ID {bikeId}: {response.ReasonPhrase}";
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(jsonContent);

        // Pretty format the JSON response
        return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    [McpServerTool, Description("Create a new bike order.")]
    public static async Task<string> CreateBikeOrder(
        HttpClient client,
        [Description("The ID of the bike to order")] int bikeId,
        [Description("Customer's full name")] string customerName,
        [Description("Customer's email address")] string customerEmail,
        [Description("Shipping address for the order")] string shippingAddress)
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

        var requestUri = $"{BaseUrl}/api/orders";

        using var response = await client.PostAsync(requestUri, content);
        if (!response.IsSuccessStatusCode)
        {
            return $"Failed to create order: {response.ReasonPhrase}";
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(responseContent);

        // Pretty format the JSON response
        return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    [McpServerTool, Description("Get order details by order ID.")]
    public static async Task<string> GetOrderById(
        HttpClient client,
        [Description("The order ID to retrieve")] string orderId)
    {
        var requestUri = $"{BaseUrl}/api/orders/{orderId}";

        using var response = await client.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            return $"Failed to get order with ID {orderId}: {response.ReasonPhrase}";
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(jsonContent);

        // Pretty format the JSON response
        return JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
