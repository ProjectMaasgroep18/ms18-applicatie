using Maasgroep.Database.Members;
using Maasgroep.Database.Photos;

namespace Maasgroep.Database.Receipts
{
    internal record Receipt
	{
		public long Id { get; set; }
		public decimal? Amount { get; set; }
		public long? StoreId { get; set; }
		public long? CostCentreId { get; set; }
		public long ReceiptStatusId { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }



		// EF receipt properties
		public CostCentre? CostCentre { get; set; }
		public ReceiptStatus ReceiptStatus { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
    }
}