
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Members
{
    public record Member : MemberRecordActive
	{
        public long Id { get; set; }
        public string Name { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Permissions { get; set; }

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


		// EF Generic properties
		public ICollection<Member>? MembersCreated { get; set; }
        public ICollection<Member>? MembersModified { get; set; }
        public ICollection<Member>? MembersDeleted { get; set; }
        public ICollection<Permission> PermissionsCreated { get; set; }
        public ICollection<Permission> PermissionsModified { get; set; }
        public ICollection<Permission> PermissionsDeleted { get; set; }
        public ICollection<MemberPermission> MemberPermissionsCreated { get; set; }
        public ICollection<MemberPermission> MemberPermissionsModified { get; set; }
        public ICollection<MemberPermission> MemberPermissionsDeleted { get; set; }

    }
}
