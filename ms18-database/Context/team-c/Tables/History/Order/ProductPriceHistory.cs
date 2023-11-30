
namespace Maasgroep.Database.Order
{
    public record ProductPriceHistory : GenericRecordHistory
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }

    }
}
