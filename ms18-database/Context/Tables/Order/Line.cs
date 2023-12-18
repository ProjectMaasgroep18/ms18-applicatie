
namespace Maasgroep.Database.Orders
{
    public record Line : GenericRecordActive
	{
        public long BillId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public double Amount { get; set; }


        // Ef
        public Bill Bill { get; set; }
        public Product Product { get; set; }
    }
}
