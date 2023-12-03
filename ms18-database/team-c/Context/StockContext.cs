using Maasgroep.Database.Order;
using Maasgroep.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database
{
	public class StockContext : DbContext
	{
		private string _connectionString;

		public DbSet<Member> Member { get; set; }

		#region Order

		public DbSet<Product> Product { get; set; }
		public DbSet<Stock> Stock { get; set; }
		public DbSet<ProductPrice> ProductPrice { get; set; }
		public DbSet<Line> OrderLines { get; set; }
		public DbSet<Bill> Bills { get; set; }

		#endregion

		#region OrderHistory

		public DbSet<ProductHistory> ProductHistory { get; set; }
		public DbSet<StockHistory> StockHistory { get; set; }
		public DbSet<ProductPriceHistory> ProductPriceHistory { get; set; }
		public DbSet<LineHistory> LineHistory { get; set; }
		public DbSet<BillHistory> BillHistory { get; set; }

		#endregion

		public StockContext(ConfigurationService configurationService)
		{
			var hoi = configurationService.GetConnectionString();
			var ditte = "";
			_connectionString = "UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true";
		}

		public StockContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public StockContext(DbContextOptionsBuilder optionsBuilder)
		{
			OnConfiguring(optionsBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			CreateProduct(modelBuilder);
			CreateStock(modelBuilder);
			CreateProductPrice(modelBuilder);
			CreateOrderLine(modelBuilder);
			CreateBill(modelBuilder);

			CreateProductHistory(modelBuilder);
			CreateStockHistory(modelBuilder);
			CreateProductPriceHistory(modelBuilder);
			CreateOrderLineHistory(modelBuilder);
			CreateBillHistory(modelBuilder);

			CreateMember(modelBuilder);
		}

		private void CreateMember(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Member>().ToTable("member", "admin");
			modelBuilder.Entity<Member>().HasKey(m => m.Id);
			modelBuilder.HasSequence<long>("memberSeq", schema: "admin").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Member>().Property(m => m.Id).HasDefaultValueSql("nextval('admin.\"memberSeq\"')");
			modelBuilder.Entity<Member>().Property(m => m.DateTimeCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<Member>().Property(m => m.Name).HasMaxLength(256);
			modelBuilder.Entity<Member>().HasIndex(m => m.Name).IsUnique();

			// Foreign keys

			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberCreated)
				.WithMany(m => m.MembersCreated)
				.HasForeignKey(m => m.MemberCreatedId)
				.HasConstraintName("FK_member_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberModified)
				.WithMany(m => m.MembersModified)
				.HasForeignKey(m => m.MemberModifiedId)
				.HasConstraintName("FK_member_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberDeleted)
				.WithMany(m => m.MembersDeleted)
				.HasForeignKey(m => m.MemberDeletedId)
				.HasConstraintName("FK_member_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		#region Order
		private void CreateProduct(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasKey(p => p.Id);
			modelBuilder.Entity<Product>().ToTable("product", "order");
			modelBuilder.HasSequence<long>("productSeq", schema: "order").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Product>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("nextval('order.\"productSeq\"')");
			modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(2048);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.MemberCreated)
				.WithMany(m => m.ProductsCreated)
				.HasForeignKey(p => p.MemberCreatedId)
				.HasConstraintName("FK_orderProduct_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.MemberModified)
				.WithMany(m => m.ProductsModified)
				.HasForeignKey(p => p.MemberModifiedId)
				.HasConstraintName("FK_orderProduct_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.MemberDeleted)
				.WithMany(m => m.ProductsDeleted)
				.HasForeignKey(p => p.MemberDeletedId)
				.HasConstraintName("FK_orderProduct_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		private void CreateStock(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Stock>().HasKey(s => s.ProductId);
			modelBuilder.Entity<Stock>().ToTable("stock", "order");
			modelBuilder.Entity<Stock>().Property(s => s.DateTimeCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<Stock>().ToTable(s => s.HasCheckConstraint("CK_order_quantity", "\"Quantity\" >= 0"));

			modelBuilder.Entity<Stock>()
				.HasOne(s => s.Product)
				.WithOne(p => p.Stock)
				.HasForeignKey<Stock>(s => s.ProductId)
				.HasConstraintName("FK_orderStock_product")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Stock>()
				.HasOne(s => s.MemberCreated)
				.WithMany(m => m.StocksCreated)
				.HasForeignKey(s => s.MemberCreatedId)
				.HasConstraintName("FK_orderStock_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Stock>()
				.HasOne(s => s.MemberModified)
				.WithMany(m => m.StocksModified)
				.HasForeignKey(s => s.MemberModifiedId)
				.HasConstraintName("FK_orderStock_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Stock>()
				.HasOne(s => s.MemberDeleted)
				.WithMany(m => m.StocksDeleted)
				.HasForeignKey(s => s.MemberDeletedId)
				.HasConstraintName("FK_orderStock_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		private void CreateOrderLine(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Line>().HasKey(pp => pp.Id);
			modelBuilder.Entity<Line>().ToTable("line", "order");
			modelBuilder.Entity<Line>().ToTable(pp => pp.HasCheckConstraint("CK_orderLine_quantity", "\"Quantity\" > 0"));
			modelBuilder.Entity<Line>().Property(pp => pp.DateTimeCreated).HasDefaultValueSql("now()");


			modelBuilder.Entity<Line>()
				.HasOne(ol => ol.Product)
				.WithMany(p => p.OrderLines)
				.HasForeignKey(ol => ol.ProductId)
				.HasConstraintName("FK_orderLine_product")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Line>()
				.HasOne(ol => ol.Bill)
				.WithMany(b => b.Lines)
				.HasForeignKey(ol => ol.BillId)
				.HasConstraintName("FK_orderLine_bill")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Line>()
				.HasOne(ol => ol.MemberCreated)
				.WithMany(m => m.LinesCreated)
				.HasForeignKey(ol => ol.MemberCreatedId)
				.HasConstraintName("FK_orderLine_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Line>()
				.HasOne(ol => ol.MemberModified)
				.WithMany(m => m.LinesModified)
				.HasForeignKey(ol => ol.MemberModifiedId)
				.HasConstraintName("FK_orderLine_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Line>()
				.HasOne(ol => ol.MemberDeleted)
				.WithMany(m => m.LinesDeleted)
				.HasForeignKey(ol => ol.MemberDeletedId)
				.HasConstraintName("FK_orderLine_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		private void CreateProductPrice(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductPrice>().HasKey(pp => pp.ProductId);
			modelBuilder.Entity<ProductPrice>().ToTable("productPrice", "order");
			modelBuilder.Entity<ProductPrice>().ToTable(pp => pp.HasCheckConstraint("CK_orderProductPrice_price", "\"Price\" >= 0"));
			modelBuilder.Entity<ProductPrice>().Property(pp => pp.DateTimeCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<ProductPrice>().Property(pp => pp.Price).HasPrecision(18, 2);

			modelBuilder.Entity<ProductPrice>()
				.HasOne(pp => pp.Product)
				.WithOne(p => p.ProductPrice)
				.HasForeignKey<ProductPrice>(pp => pp.ProductId)
				.HasConstraintName("FK_orderProductPrice_product")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ProductPrice>()
				.HasOne(pp => pp.MemberCreated)
				.WithMany(m => m.ProductPricesCreated)
				.HasForeignKey(pp => pp.MemberCreatedId)
				.HasConstraintName("FK_orderProductPrice_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ProductPrice>()
				.HasOne(pp => pp.MemberModified)
				.WithMany(m => m.ProductPricesModified)
				.HasForeignKey(pp => pp.MemberModifiedId)
				.HasConstraintName("FK_orderProductPrice_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ProductPrice>()
				.HasOne(pp => pp.MemberDeleted)
				.WithMany(m => m.ProductPricesDeleted)
				.HasForeignKey(pp => pp.MemberDeletedId)
				.HasConstraintName("FK_orderProductPrice_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		private void CreateBill(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Bill>().HasKey(b => b.Id);
			modelBuilder.Entity<Bill>().ToTable("bill", "order");
			modelBuilder.Entity<Bill>().Property(b => b.Name).HasMaxLength(2048);
			modelBuilder.Entity<Bill>().Property(b => b.Note).HasMaxLength(2048);
			modelBuilder.Entity<Bill>().Property(b => b.DateTimeCreated).HasDefaultValueSql("now()");

			modelBuilder.Entity<Bill>()
				.HasOne(b => b.Member)
				.WithMany(m => m.BillsOwned)
				.HasForeignKey(b => b.MemberId)
				.HasConstraintName("FK_orderBill_memberOwned")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Bill>()
				.HasOne(b => b.MemberCreated)
				.WithMany(m => m.BillsCreated)
				.HasForeignKey(b => b.MemberCreatedId)
				.HasConstraintName("FK_orderBill_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Bill>()
				.HasOne(b => b.MemberModified)
				.WithMany(m => m.BillsModified)
				.HasForeignKey(b => b.MemberModifiedId)
				.HasConstraintName("FK_orderBill_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Bill>()
				.HasOne(b => b.MemberDeleted)
				.WithMany(m => m.BillsDeleted)
				.HasForeignKey(b => b.MemberDeletedId)
				.HasConstraintName("FK_orderBill_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		#endregion

		#region Order History

		private void CreateProductHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductHistory>().ToTable("product", "orderHistory");
			modelBuilder.HasSequence<long>("productSeq", schema: "orderHistory").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<ProductHistory>().Property(p => p.Id).HasDefaultValueSql("nextval('\"orderHistory\".\"productSeq\"')");
			modelBuilder.Entity<ProductHistory>().Property(p => p.RecordCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<ProductHistory>().Property(p => p.Name).HasMaxLength(2048);
		}

		private void CreateStockHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<StockHistory>().ToTable("stock", "orderHistory");
			modelBuilder.HasSequence<long>("stockSeq", schema: "orderHistory").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<StockHistory>().Property(s => s.Id).HasDefaultValueSql("nextval('\"orderHistory\".\"stockSeq\"')");
			modelBuilder.Entity<StockHistory>().Property(s => s.RecordCreated).HasDefaultValueSql("now()");
		}

		private void CreateOrderLineHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LineHistory>().ToTable("line", "orderHistory");
			modelBuilder.HasSequence<long>("lineSeq", schema: "orderHistory").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<LineHistory>().Property(l => l.Id).HasDefaultValueSql("nextval('\"orderHistory\".\"lineSeq\"')");
			modelBuilder.Entity<LineHistory>().Property(l => l.RecordCreated).HasDefaultValueSql("now()");
		}

		private void CreateProductPriceHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductPriceHistory>().ToTable("productPrice", "orderHistory");
			modelBuilder.HasSequence<long>("productPriceSeq", schema: "orderHistory").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<ProductPriceHistory>().Property(pp => pp.Id).HasDefaultValueSql("nextval('\"orderHistory\".\"productPriceSeq\"')");
			modelBuilder.Entity<ProductPriceHistory>().Property(pp => pp.RecordCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<ProductPriceHistory>().Property(pp => pp.Price).HasPrecision(18, 2);
		}

		private void CreateBillHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BillHistory>().ToTable("bill", "orderHistory");
			modelBuilder.HasSequence<long>("billSeq", schema: "orderHistory").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<BillHistory>().Property(b => b.Id).HasDefaultValueSql("nextval('\"orderHistory\".\"billSeq\"')");
			modelBuilder.Entity<BillHistory>().Property(b => b.RecordCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<BillHistory>().Property(p => p.Name).HasMaxLength(2048);
			modelBuilder.Entity<BillHistory>().Property(b => b.Note).HasMaxLength(2048);
		}


		#endregion
	}
}
