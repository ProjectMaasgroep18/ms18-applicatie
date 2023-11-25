using Maasgroep.Database.Context;

namespace Maasgroep.Database.Receipts
{
    public record ReceiptApprovalHistory : GenericRecordHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ReceiptId { get; set; }
        public string? Note { get; set; }
    }
}
