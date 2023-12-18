
namespace Maasgroep.SharedKernel.DataModels.Receipts
{
    public record ReceiptApprovalData
    {
        public long ReceiptId { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public bool Paid { get; set; }
    }
}
