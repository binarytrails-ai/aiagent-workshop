using System.Reflection;
using System.Text.Json;
using ContosoStore.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoStore.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private const string CatalogFile = "bikes_catalog.json";

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet("bikes")]
        public async Task<IActionResult> GetAllBikes()
        {
            var bikes = await GetBikes();
            return Ok(bikes);
        }

        [HttpGet("bikes/{id:int}")]
        public async Task<IActionResult> GetBikeById(int id)
        {
            var bikes = await GetBikes();
            var bike = bikes.FirstOrDefault(b => b.Id == id);
            if (bike != null)
                return Ok(bike);
            return NotFound(new { message = "Bike not found" });
        }

        [HttpPost("orders")]
        public async Task<IActionResult> SubmitOrder([FromBody] BikeOrderRequest order)
        {
            var orderId = Guid.NewGuid().ToString();
            _logger.LogInformation($"Order submitted: {orderId}, Customer: {order?.CustomerName}, BikeId: {order?.BikeId}");
            return Ok(new { orderId, status = "Submitted" });
        }

        [HttpGet("orders/{orderId}")]
        public IActionResult GetOrderStatus(string orderId)
        {
            var statuses = new[] { "OrderReceived", "Shipped", "Delivered" };
            var random = new Random();
            var status = statuses[random.Next(statuses.Length)];
            return Ok(new { orderId, status });
        }

        private async Task<List<BikeCatalogItem>> GetBikes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(CatalogFile, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                _logger.LogError($"Embedded resource '{CatalogFile}' not found.");
                return new List<BikeCatalogItem>();
            }

            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    _logger.LogError($"Failed to open embedded resource stream for '{resourceName}'.");
                    return new List<BikeCatalogItem>();
                }
                var bikes = await JsonSerializer.DeserializeAsync<List<BikeCatalogItem>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return bikes ?? new List<BikeCatalogItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read bikes catalog from embedded resource.");
                return new List<BikeCatalogItem>();
            }
        }
    }
}
