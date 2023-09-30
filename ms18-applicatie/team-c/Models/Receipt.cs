using ms18_applicatie.Models;

namespace ms18_applicatie.Models
{

	// Waarom niet gewoon zo (in plaats van een speciale "ReceiptStatus[es]" database tabel)?
	public enum ReceiptStatusSUGGESTIE {
		Incomplete,
		Submitted,
		Approved,
		Rejected,
	}

	public record Receipt
	{
		public long Id { get; set; }
		public decimal? Amount { get; set; }
		public long? StoreId { get; set; }
		public long? CostCentreId { get; set; }
		public long ReceiptStatusId { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }


		// Generic
		public long UserCreatedId { get; set; }
		public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }



		// EF receipt properties
		public Store? Store { get; set; }
		public CostCentre? CostCentre { get; set; }
		public ReceiptStatus ReceiptStatus { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }


		// EF generic properties
		public Member UserCreated { get; set; }
		public Member? UserModified { get; set; }
	}
}