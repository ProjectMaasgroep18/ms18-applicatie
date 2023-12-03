
namespace Maasgroep.Database.Order
{
    public record ProductPrice : GenericRecordActive
	{
        public long ProductId { get; set; }
        public decimal Price { get; set; }

        // Ef
        public Product Product { get; set; }
    }
}
