using Maasgroep.Database.Members;

namespace Maasgroep.Database.Receipts
{
    internal record ReceiptApproval
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


		// EF receipt properties
		public Receipt Receipt { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
    }
}
