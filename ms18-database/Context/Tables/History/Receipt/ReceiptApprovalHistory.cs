
namespace Maasgroep.Database.Receipts
{
    public record ReceiptApprovalHistory : GenericRecordHistory
    {
        public long ReceiptId { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
    }
}
