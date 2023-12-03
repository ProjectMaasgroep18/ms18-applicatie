
namespace Maasgroep.Database.Order
{
	public record Member : OrderRecordActive
	{
		public long Id { get; set; }
		public string Name { get; set; }


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


		// EF generic properties
		public ICollection<Member>? MembersCreated { get; set; }
		public ICollection<Member>? MembersModified { get; set; }
		public ICollection<Member>? MembersDeleted { get; set; }
	}
}
