namespace ContosoStore.API.Models
{
    public class BikeOrderRequest
    {
        public int BikeId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
    }

}
