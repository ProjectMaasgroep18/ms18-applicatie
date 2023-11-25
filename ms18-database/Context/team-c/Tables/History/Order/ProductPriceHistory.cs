
namespace Maasgroep.Database.Order
{
    public record ProductPriceHistory : GenericRecordHistory
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }

    }
}
