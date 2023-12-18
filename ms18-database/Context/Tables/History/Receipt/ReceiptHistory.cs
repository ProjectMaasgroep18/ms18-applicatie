
namespace Maasgroep.Database.Receipts
{
    public record ReceiptHistory : GenericRecordHistory
    {
        public long ReceiptId { get; set; }
        public decimal? Amount { get; set; }
        public long? CostCentreId { get; set; }
        public string ReceiptStatus { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
    }
}
