using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ContosoBikestore.MCPServer.Tools;

[McpServerToolType]
public sealed class OrderManagerTool
{
    private readonly ILogger<OrderManagerTool> _logger;
    private readonly Random _random = new Random();
    
    // Simulated delivery partners
    private readonly string[] _deliveryPartners = { 
        "Speedy Delivery", 
        "BikeShipper Pro", 
        "Express Courier", 
        "CycleFreight"
    };
    
    // Simulated order statuses
    private readonly string[] _orderStatuses = { 
        "Processing", 
        "Confirmed", 
        "Preparing for Shipment", 
        "Shipped", 
        "Out for Delivery", 
        "Delivered", 
        "Delayed"
    };

    public OrderManagerTool(ILogger<OrderManagerTool> logger)
    {
        _logger = logger;
    }

    [McpServerTool, Description("Check the status of an order with delivery estimates")]
    public Task<string> CheckOrderStatus(
        [Description("The order ID to check")] string orderId)
    {
        try
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return Task.FromResult("Please provide an order ID.");
            }

            // Generate deterministic but seemingly random data based on the order ID
            var orderIdSeed = orderId.GetHashCode();
            var rand = new Random(orderIdSeed);
            
            // Create a simulated status based on the order ID
            var statusIndex = rand.Next(0, _orderStatuses.Length);
            var status = _orderStatuses[statusIndex];
            
            // Calculate dates based on the status
            var orderDate = DateTime.Now.AddDays(-rand.Next(1, 10));
            var estimatedDeliveryDate = orderDate.AddDays(rand.Next(3, 14));
            var actualDeliveryDate = status == "Delivered" ? 
                orderDate.AddDays(rand.Next(3, 10)) : 
                (DateTime?)null;
                
            // Choose a delivery partner
            var deliveryPartner = _deliveryPartners[rand.Next(0, _deliveryPartners.Length)];
            
            // Generate a tracking number
            var trackingNumber = $"TRK-{rand.Next(100000, 999999)}";
            
            // Create status updates
            var statusUpdates = new List<object>();
            switch (status)
            {
                case "Processing":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    break;
                case "Confirmed":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    break;
                case "Preparing for Shipment":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(1).AddHours(rand.Next(1, 8)), 
                        Status = "Preparing for Shipment", 
                        Message = "Your order is being prepared for shipment." 
                    });
                    break;
                case "Shipped":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(1).AddHours(rand.Next(1, 8)), 
                        Status = "Preparing for Shipment", 
                        Message = "Your order is being prepared for shipment." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(2).AddHours(rand.Next(1, 8)), 
                        Status = "Shipped", 
                        Message = $"Your order has been shipped with {deliveryPartner}. Tracking number: {trackingNumber}" 
                    });
                    break;
                case "Out for Delivery":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(1).AddHours(rand.Next(1, 8)), 
                        Status = "Preparing for Shipment", 
                        Message = "Your order is being prepared for shipment." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(2).AddHours(rand.Next(1, 8)), 
                        Status = "Shipped", 
                        Message = $"Your order has been shipped with {deliveryPartner}. Tracking number: {trackingNumber}" 
                    });
                    statusUpdates.Add(new { 
                        Date = DateTime.Now, 
                        Status = "Out for Delivery", 
                        Message = "Your order is out for delivery and will arrive today." 
                    });
                    break;
                case "Delivered":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(1).AddHours(rand.Next(1, 8)), 
                        Status = "Preparing for Shipment", 
                        Message = "Your order is being prepared for shipment." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(2).AddHours(rand.Next(1, 8)), 
                        Status = "Shipped", 
                        Message = $"Your order has been shipped with {deliveryPartner}. Tracking number: {trackingNumber}" 
                    });
                    statusUpdates.Add(new { 
                        Date = actualDeliveryDate.Value.AddHours(-5), 
                        Status = "Out for Delivery", 
                        Message = "Your order is out for delivery and will arrive today." 
                    });
                    statusUpdates.Add(new { 
                        Date = actualDeliveryDate.Value, 
                        Status = "Delivered", 
                        Message = "Your order has been delivered. Thank you for shopping with Contoso Bikes!" 
                    });
                    break;
                case "Delayed":
                    statusUpdates.Add(new { 
                        Date = orderDate, 
                        Status = "Order Received", 
                        Message = "Your order has been received and is being processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddHours(rand.Next(1, 5)), 
                        Status = "Order Confirmed", 
                        Message = "Your order has been confirmed and payment has been processed." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(1).AddHours(rand.Next(1, 8)), 
                        Status = "Preparing for Shipment", 
                        Message = "Your order is being prepared for shipment." 
                    });
                    statusUpdates.Add(new { 
                        Date = orderDate.AddDays(2).AddHours(rand.Next(1, 8)), 
                        Status = "Shipped", 
                        Message = $"Your order has been shipped with {deliveryPartner}. Tracking number: {trackingNumber}" 
                    });
                    statusUpdates.Add(new { 
                        Date = DateTime.Now.AddDays(-1), 
                        Status = "Delayed", 
                        Message = "Your delivery has been delayed. We apologize for the inconvenience." 
                    });
                    estimatedDeliveryDate = DateTime.Now.AddDays(rand.Next(2, 5));
                    break;
            }

            var result = new
            {
                OrderId = orderId,
                CurrentStatus = status,
                OrderDate = orderDate,
                EstimatedDeliveryDate = estimatedDeliveryDate,
                ActualDeliveryDate = actualDeliveryDate,
                DeliveryPartner = deliveryPartner,
                TrackingNumber = trackingNumber,
                StatusUpdates = statusUpdates
            };

            return Task.FromResult(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OrderSimulationTools] Exception occurred in CheckOrderStatus");
            throw;
        }
    }

    [McpServerTool, Description("Generate a simulated delivery estimate for a bike order")]
    public Task<string> GetDeliveryEstimate(
        [Description("ID of the bike being ordered")] int bikeId,
        [Description("Delivery city")] string city,
        [Description("Whether to request express shipping")] bool expressShipping = false)
    {
        try
        {
            // Calculate base shipping days
            var baseShippingDays = _random.Next(3, 8);
            
            // Adjust for express shipping
            var shippingDays = expressShipping ? 
                Math.Max(1, baseShippingDays - _random.Next(1, 3)) : 
                baseShippingDays;
                
            // Calculate estimated dates
            var orderDate = DateTime.Now;
            var processingDate = orderDate.AddDays(1);
            var shippingDate = processingDate.AddDays(_random.Next(1, 3));
            var deliveryDate = shippingDate.AddDays(shippingDays);
            
            // Calculate shipping cost
            var baseShippingCost = _random.Next(15, 40);
            var expressMultiplier = expressShipping ? 2.5m : 1.0m;
            var shippingCost = Math.Round(baseShippingCost * expressMultiplier, 2);
            
            // Delivery partner and options
            var deliveryPartner = _deliveryPartners[_random.Next(_deliveryPartners.Length)];
            var deliveryOptions = new List<object>
            {
                new {
                    Option = "Standard Delivery",
                    Cost = baseShippingCost,
                    EstimatedDays = baseShippingDays,
                    SelectedByDefault = !expressShipping
                },
                new {
                    Option = "Express Delivery",
                    Cost = Math.Round(baseShippingCost * 2.5m, 2),
                    EstimatedDays = Math.Max(1, baseShippingDays - _random.Next(1, 3)),
                    SelectedByDefault = expressShipping
                }
            };

            var result = new
            {
                BikeId = bikeId,
                DeliveryCity = city,
                ShippingType = expressShipping ? "Express" : "Standard",
                Timeline = new
                {
                    OrderDate = orderDate,
                    ProcessingDate = processingDate,
                    ShippingDate = shippingDate,
                    EstimatedDeliveryDate = deliveryDate
                },
                ShippingDetails = new
                {
                    DeliveryPartner = deliveryPartner,
                    ShippingCost = $"${shippingCost}",
                    EstimatedDeliveryDays = shippingDays
                },
                AvailableOptions = deliveryOptions
            };

            return Task.FromResult(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OrderSimulationTools] Exception occurred in GetDeliveryEstimate");
            throw;
        }
    }
}