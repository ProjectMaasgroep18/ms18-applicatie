
namespace Maasgroep.Database.Orders
{
    public record BillHistory : GenericRecordHistory
    {
        public long BillId { get; set; }
        public bool IsGuest { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public double TotalAmount { get; set; }
    }
}
