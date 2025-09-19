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

    [McpServerTool, Description("Submit a new order for a bike")]
    public Task<string> SubmitOrder(
        [Description("The ID of the bike being ordered")] int bikeId,
        [Description("The email address of the customer")] string emailAddress,
        [Description("The shipping address for delivery")] string shippingAddress)
    {
        try
        {
            // Validate inputs
            if (bikeId <= 0)
            {
                return Task.FromResult("Please provide a valid bike ID.");
            }
            
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return Task.FromResult("Please provide a valid email address.");
            }
            
            if (string.IsNullOrWhiteSpace(shippingAddress))
            {
                return Task.FromResult("Please provide a valid shipping address.");
            }
            
            // Generate a unique order ID
            string orderId = $"ORD-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            
            // Generate random order date (for simulation purposes)
            var orderDate = DateTime.Now;
            
            // Calculate estimated delivery date (random between 3-10 days from now)
            var estimatedDeliveryDate = orderDate.AddDays(_random.Next(3, 11));
            
            // Choose a delivery partner
            var deliveryPartner = _deliveryPartners[_random.Next(0, _deliveryPartners.Length)];
            
            // Create the initial status update
            var statusUpdates = new List<object>
            {
                new { 
                    Date = orderDate, 
                    Status = "Order Received", 
                    Message = "Your order has been received and is being processed." 
                }
            };
            
            // Create the order response
            var result = new
            {
                OrderId = orderId,
                BikeId = bikeId,
                CustomerEmail = emailAddress,
                ShippingAddress = shippingAddress,
                CurrentStatus = "Processing",
                OrderDate = orderDate,
                EstimatedDeliveryDate = estimatedDeliveryDate,
                DeliveryPartner = deliveryPartner,
                StatusUpdates = statusUpdates,
                Message = "Your order has been successfully submitted. Use the order ID to check the status."
            };
            
            _logger.LogInformation($"[OrderManagerTool] New order submitted - OrderId: {orderId}, BikeId: {bikeId}, Email: {emailAddress}");
            
            return Task.FromResult(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OrderManagerTool] Exception occurred in SubmitOrder");
            throw;
        }
    }
}
