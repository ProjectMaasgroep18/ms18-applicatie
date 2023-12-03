
namespace Maasgroep.Database.Receipts
{
    public record ReceiptApproval : ReceiptActiveRecord
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }
        public bool Approved { get; set; }

        // EF receipt properties
        public Receipt Receipt { get; set; }
    }
}
