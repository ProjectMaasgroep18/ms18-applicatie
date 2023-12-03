
namespace Maasgroep.Database.Order
{
    public record Line : OrderRecordActive
	{
        public long Id { get; set; }
        public long BillId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }


        // Ef
        public Bill Bill { get; set; }
        public Product Product { get; set; }
    }
}
