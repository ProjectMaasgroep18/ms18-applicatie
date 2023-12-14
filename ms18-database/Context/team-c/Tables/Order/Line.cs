
namespace Maasgroep.Database.Orders
{
    public record Line : GenericRecordActive
	{
        public long BillId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }


        // Ef
        public Bill Bill { get; set; }
        public Product Product { get; set; }
    }
}
