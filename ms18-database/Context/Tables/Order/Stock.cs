
namespace Maasgroep.Database.Orders
{
    public record Stock : GenericRecordActive
    {
        public long Quantity { get; set; }

        // EF
        public Product Product { get; set; }
    }
}
