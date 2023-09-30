
namespace Maasgroep.Database
{
	public record ReceiptApproval
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }


        // Generic
        public long UserCreatedId { get; set; }
        public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


		// EF receipt properties
		public Receipt Receipt { get; set; }


		// EF generic properties
		public Member UserCreated { get; set; }
		public Member? UserModified { get; set; }
	}
}
