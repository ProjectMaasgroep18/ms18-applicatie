
namespace Maasgroep.SharedKernel.DataModels.Orders
{
    public record ProductData
    {
        public string Name { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public double Price { get; set; }
        public int PriceQuantity { get; set; }
        public long? Stock { get; set; }
    }
}
