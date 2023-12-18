
namespace Maasgroep.Database.Orders
{
    public record LineHistory : GenericRecordHistory
    {
        public long LineId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long? MemberId { get; set; }
        public bool IsGuest { get; set; }
        public string? Note { get; set; }
    }
}
