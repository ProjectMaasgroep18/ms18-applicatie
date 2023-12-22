using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Admin
{
    public record Member : GenericRecordActive
    {
        public string Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Permissions { get; set; }
        public ICollection<TokenStore> Token { get; set; }

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
        public ICollection<ReceiptPhoto> PhotosCreated { get; set; }
        public ICollection<ReceiptPhoto> PhotosModified { get; set; }
        public ICollection<ReceiptPhoto> PhotosDeleted { get; set; }

        // EF Order properties
        public ICollection<Product> ProductsCreated { get; set; }
        public ICollection<Product> ProductsModified { get; set; }
        public ICollection<Product> ProductsDeleted { get; set; }
        public ICollection<Stock> StocksCreated { get; set; }
        public ICollection<Stock> StocksModified { get; set; }
        public ICollection<Stock> StocksDeleted { get; set; }
        public ICollection<Line> LinesCreated { get; set; }
        public ICollection<Line> LinesModified { get; set; }
        public ICollection<Line> LinesDeleted { get; set; }
        public ICollection<Bill> BillsOwned { get; set; }
        public ICollection<Bill> BillsCreated { get; set; }
        public ICollection<Bill> BillsModified { get; set; }
        public ICollection<Bill> BillsDeleted { get; set; }

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
        public ICollection<TokenStore> TokenStoresCreated { get; set; }
        public ICollection<TokenStore> TokenStoresModified { get; set; }
        public ICollection<TokenStore> TokenStoresDeleted { get; set; }

    }
}
