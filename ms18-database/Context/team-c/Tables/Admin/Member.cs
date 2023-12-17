using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.ToDoList;

namespace Maasgroep.Database.Members
{
    public record Member
	{
        public long Id { get; set; }
        public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }


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
		public ICollection<ProductPrice> ProductPricesCreated { get; set; }
		public ICollection<ProductPrice> ProductPricesModified { get; set; }
		public ICollection<ProductPrice> ProductPricesDeleted { get; set; }
		public ICollection<Bill> BillsOwned { get; set; }
		public ICollection<Bill> BillsCreated { get; set; }
		public ICollection<Bill> BillsModified { get; set; }
		public ICollection<Bill> BillsDeleted { get; set; }

		// EF ToDo properties
		public ICollection<ToDo> ToDoOwned { get; set; }
		public ICollection<ToDo> ToDoCreated { get; set; }
		public ICollection<ToDo> ToDoModified { get; set; }
		public ICollection<ToDo> ToDoDeleted { get; set; }


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

		// Generic
		public long? MemberCreatedId { get; set; }
		public long? MemberModifiedId { get; set; }
		public long? MemberDeletedId { get; set; }
		public DateTime DateTimeCreated { get; set; }
		public DateTime? DateTimeModified { get; set; }
		public DateTime? DateTimeDeleted { get; set; }

		// EF generic properties
		public Member? MemberCreated { get; set; }
		public Member? MemberModified { get; set; }
		public Member? MemberDeleted { get; set; }

	}
}
