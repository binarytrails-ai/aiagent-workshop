namespace ContosoStore.API.Models
{
    public class BikeCatalogItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal PriceUSD { get; set; }
        public List<string> Tags { get; set; }
        public string City { get; set; }
        public string Branch { get; set; }
        public int StockAvailability { get; set; }
    }

}
