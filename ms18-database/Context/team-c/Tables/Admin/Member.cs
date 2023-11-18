using Maasgroep.Database.Receipts;
using Maasgroep.Database.Stock;

namespace Maasgroep.Database.Members
{
    public record Member
    {
        public long Id { get; set; }
        public string Name { get; set; }


        // Generic
        public long? MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF receipt properties
        public ICollection<CostCentre> CostCentresCreated { get; set; }
        public ICollection<CostCentre> CostCentresModified { get; set; }
        public ICollection<CostCentre> CostCentresDeleted { get; set; }
        public ICollection<ReceiptStatus> ReceiptStatusesCreated { get; set; }
        public ICollection<ReceiptStatus> ReceiptStatusesModified { get; set; }
        public ICollection<ReceiptStatus> ReceiptStatusesDeleted { get; set; }
        public ICollection<Receipt> ReceiptsCreated { get; set; }
        public ICollection<Receipt> ReceiptsModified { get; set; }
        public ICollection<Receipt> ReceiptsDeleted { get; set; }
        public ICollection<ReceiptApproval> ReceiptApprovalsCreated { get; set; }
        public ICollection<ReceiptApproval> ReceiptApprovalsModified { get; set; }
        public ICollection<ReceiptApproval> ReceiptApprovalsDeleted { get; set; }

        // EF Stock properties
        public ICollection<Product> ProductsCreated { get; set; }
        public ICollection<Product> ProductsModified { get; set; }
        public ICollection<Product> ProductsDeleted { get; set; }
        public ICollection<Stockpile> StocksCreated { get; set; }
        public ICollection<Stockpile> StocksModified { get; set; }
        public ICollection<Stockpile> StocksDeleted { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Permissions { get; set; }

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


        // EF generic properties
        public Member? MemberCreated { get; set; }
        public Member? MemberModified { get; set; }

    }
}
