
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database.Receipts
{
	[Table("member")]
	public record Member
	{
		public long Id { get; set; }
		public string Name { get; set; }


		// Generic
		public long? MemberCreatedId { get; set; }
		public long? MemberModifiedId { get; set; }
		public long? MemberDeletedId { get; set; }
		public DateTime? DateTimeCreated { get; set; }
		public DateTime? DateTimeModified { get; set; }
		public DateTime? DateTimeDeleted { get; set; }


		// EF receipt properties
		public ICollection<CostCentre> CostCentresCreated { get; set; }
		public ICollection<CostCentre> CostCentresModified { get; set; }
		public ICollection<CostCentre> CostCentresDeleted { get; set; }
		public ICollection<Receipt> ReceiptsCreated { get; set; }
		public ICollection<Receipt> ReceiptsModified { get; set; }
		public ICollection<Receipt> ReceiptsDeleted { get; set; }
		public ICollection<ReceiptApproval> ReceiptApprovalsCreated { get; set; }
		public ICollection<ReceiptApproval> ReceiptApprovalsModified { get; set; }
		public ICollection<ReceiptApproval> ReceiptApprovalsDeleted { get; set; }
		public ICollection<Photo> PhotosCreated { get; set; }
		public ICollection<Photo> PhotosModified { get; set; }
		public ICollection<Photo> PhotosDeleted { get; set; }

		// EF generic properties
		public Member? MemberCreated { get; set; }
		public Member? MemberModified { get; set; }
		public Member? MemberDeleted { get; set; }
		public ICollection<Member>? MembersCreated { get; set; }
		public ICollection<Member>? MembersModified { get; set; }
		public ICollection<Member>? MembersDeleted { get; set; }
	}
}
