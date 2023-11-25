
namespace Maasgroep.Database.Receipts
{
    public record ReceiptHistory : GenericRecordHistory
    {
        public long ReceiptId { get; set; }
        public decimal? Amount { get; set; }
        public long? StoreId { get; set; }
        public long? CostCentreId { get; set; }
        public long ReceiptStatusId { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
    }
}
