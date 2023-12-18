
namespace Maasgroep.Database.Orders
{
    public record ProductHistory : GenericRecordHistory
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public double Price { get; set; }
        public int PriceQuantity { get; set; }
    }
}
