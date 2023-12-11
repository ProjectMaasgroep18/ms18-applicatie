
namespace Maasgroep.Database.Receipts
{
    public record ReceiptApproval : GenericRecordActive
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }
        public bool Approved { get; set; }

        // EF receipt properties
        public Receipt Receipt { get; set; }
    }
}
