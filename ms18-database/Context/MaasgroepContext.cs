using Maasgroep.Database.Context.team_d.Models;
using Maasgroep.Database.Members;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.ToDoList;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Context
{
    public class MaasgroepContext : DbContext
    {
        // Used for dependency injection
        public MaasgroepContext(DbContextOptions<MaasgroepContext> options) : base(options)
        {
        }

        public MaasgroepContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions<MaasgroepContext> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MaasgroepContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }

        #region Member

        public DbSet<Member> Member { get; set; } = null!;
        public DbSet<Permission> Permission { get; set; } = null!;
        public DbSet<MemberPermission> MemberPermission { get; set; } = null!;
        #endregion

        #region Receipts
        public DbSet<Receipt> Receipt { get; set; } = null!;
        public DbSet<ReceiptApproval> ReceiptApproval { get; set; } = null!;
        public DbSet<CostCentre> CostCentre { get; set; } = null!;
        public DbSet<ReceiptPhoto> ReceiptPhoto { get; set; } = null!;
        #endregion

        #region ReceiptHistory

        public DbSet<CostCentreHistory> CostCentreHistory { get; set; } = null!;
        public DbSet<ReceiptApprovalHistory> ReceiptApprovalHistory { get; set; } = null!;
        public DbSet<ReceiptHistory> ReceiptHistory { get; set; } = null!;
        public DbSet<PhotoHistory> PhotoHistory { get; set; } = null!;

        #endregion

        #region Order

        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<Stock> Stock { get; set; } = null!;
        public DbSet<ProductPrice> ProductPrice { get; set; } = null!;
        public DbSet<Line> OrderLines { get; set; } = null!;
        public DbSet<Bill> Bills { get; set; } = null!;

        #endregion

        #region OrderHistory

        public DbSet<ProductHistory> ProductHistory { get; set; } = null!;
        public DbSet<StockHistory> StockHistory { get; set; } = null!;
        public DbSet<ProductPriceHistory> ProductPriceHistory { get; set; } = null!;
        public DbSet<LineHistory> LineHistory { get; set; } = null!;
        public DbSet<BillHistory> BillHistory { get; set; } = null!;

        #endregion

        #region PhotoAlbum

        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<AlbumTag> AlbumTags { get; set; } = null!;

        #endregion

        #region Todo
        public DbSet<ToDo> ToDos { get; set; } = null!;
        public DbSet<ToDoHistory> ToDoHistory { get; set; } = null!;
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateMember(modelBuilder);
            CreatePermission(modelBuilder);
            CreateMemberPermission(modelBuilder);

            #region Receipt
            CreateReceipt(modelBuilder);
            CreateReceiptApproval(modelBuilder);
            CreateCostCentre(modelBuilder);
            CreatePhoto(modelBuilder);

            CreateReceiptHistory(modelBuilder);
            CreateReceiptApprovalHistory(modelBuilder);
            CreateCostCentreHistory(modelBuilder);
            CreatePhotoHistory(modelBuilder);
            #endregion

            #region Order
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
            #endregion

            #region PhotoAlbum

            CreateAlbums(modelBuilder);
            CreatePhotos(modelBuilder);
            CreateTags(modelBuilder);
            CreateLikes(modelBuilder);
            CreateAlbumTags(modelBuilder);

            #endregion

            #region Todo
            CreateToDo(modelBuilder);
            CreateToDoHistory(modelBuilder);
            #endregion
        }

        #region Member
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

        private void CreatePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().ToTable("permission", "admin");
            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            modelBuilder.HasSequence<long>("permissionSeq", schema: "admin").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Permission>().Property(p => p.Id).HasDefaultValueSql("nextval('admin.\"permissionSeq\"')");
            modelBuilder.Entity<Permission>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Permission>().Property(p => p.Name).HasMaxLength(256);
            modelBuilder.Entity<Permission>().HasIndex(p => p.Name).IsUnique();

            // Foreign keys

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.MemberCreated)
                .WithMany(m => m.PermissionsCreated)
                .HasForeignKey(p => p.MemberCreatedId)
                .HasConstraintName("FK_permission_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.MemberModified)
                .WithMany(m => m.PermissionsModified)
                .HasForeignKey(p => p.MemberModifiedId)
                .HasConstraintName("FK_permission_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.MemberDeleted)
                .WithMany(m => m.PermissionsDeleted)
                .HasForeignKey(p => p.MemberDeletedId)
                .HasConstraintName("FK_permission_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);


        }

        private void CreateMemberPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberPermission>().ToTable("memberPermission", "admin");
            modelBuilder.Entity<MemberPermission>().HasKey(mp => new { mp.MemberId, mp.PermissionId });
            modelBuilder.Entity<MemberPermission>().Property(mp => mp.DateTimeCreated).HasDefaultValueSql("now()");

            // Foreign keys

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.Permission)
                .WithMany(p => p.Members)
                .HasForeignKey(mp => mp.PermissionId)
                .HasConstraintName("FK_memberPermission_permission")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.Member)
                .WithMany(m => m.Permissions)
                .HasForeignKey(mp => mp.MemberId)
                .HasConstraintName("FK_memberPermission_member")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.MemberCreated)
                .WithMany(m => m.MemberPermissionsCreated)
                .HasForeignKey(mp => mp.MemberCreatedId)
                .HasConstraintName("FK_memberPermission_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.MemberModified)
                .WithMany(m => m.MemberPermissionsModified)
                .HasForeignKey(mp => mp.MemberModifiedId)
                .HasConstraintName("FK_memberPermission_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.MemberDeleted)
                .WithMany(m => m.MemberPermissionsDeleted)
                .HasForeignKey(mp => mp.MemberDeletedId)
                .HasConstraintName("FK_memberPermission_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }
        #endregion

        #region Receipt
        private void CreateCostCentre(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostCentre>().ToTable("costCentre", "receipt");
            modelBuilder.Entity<CostCentre>().HasKey(cc => new { cc.Id });
            modelBuilder.HasSequence<long>("costCentreSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<CostCentre>().Property(cc => cc.Id).HasDefaultValueSql("nextval('receipt.\"costCentreSeq\"')");
            modelBuilder.Entity<CostCentre>().Property(cc => cc.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<CostCentre>().Property(cc => cc.Name).HasMaxLength(256);
            modelBuilder.Entity<CostCentre>().HasIndex(cc => cc.Name).IsUnique();

            // Foreign keys

            modelBuilder.Entity<CostCentre>()
                .HasOne(cc => cc.MemberCreated)
                .WithMany(m => m.CostCentresCreated)
                .HasForeignKey(cc => cc.MemberCreatedId)
                .HasConstraintName("FK_costCentre_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CostCentre>()
                .HasOne(cc => cc.MemberModified)
                .WithMany(m => m.CostCentresModified)
                .HasForeignKey(cc => cc.MemberModifiedId)
                .HasConstraintName("FK_costCentre_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CostCentre>()
                .HasOne(cc => cc.MemberDeleted)
                .WithMany(m => m.CostCentresDeleted)
                .HasForeignKey(cc => cc.MemberDeletedId)
                .HasConstraintName("FK_costCentre_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void CreateReceipt(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>().ToTable("receipt", "receipt");
            modelBuilder.Entity<Receipt>().ToTable(t => t.HasCheckConstraint("CK_receipt_amount", "\"Amount\" >= 0"));
            modelBuilder.Entity<Receipt>().HasKey(r => new { r.Id });
            modelBuilder.HasSequence<long>("receiptSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Receipt>().Property(r => r.Id).HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");
            modelBuilder.Entity<Receipt>().Property(r => r.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Receipt>().Property<string>(r => r.Note).HasMaxLength(2048);
            modelBuilder.Entity<Receipt>().Property(r => r.Amount).HasPrecision(18, 2);

            //Foreign keys

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.CostCentre)
                .WithMany(c => c.Receipt)
                .HasForeignKey(r => r.CostCentreId)
                .HasConstraintName("FK_receipt_costCentre")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipt>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.ReceiptsCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_receipt_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipt>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.ReceiptsModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_receipt_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipt>()
                .HasOne(ra => ra.MemberDeleted)
                .WithMany(m => m.ReceiptsDeleted)
                .HasForeignKey(ra => ra.MemberDeletedId)
                .HasConstraintName("FK_receipt_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void CreateReceiptApproval(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptApproval>().ToTable("approval", "receipt");
            modelBuilder.Entity<ReceiptApproval>().HasKey(ra => new { ra.ReceiptId });
            modelBuilder.Entity<ReceiptApproval>().Property(ra => ra.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ReceiptApproval>().Property(ra => ra.Note).HasMaxLength(2048);

            //Foreign keys

            modelBuilder.Entity<ReceiptApproval>()
                .HasOne(ra => ra.Receipt)
                .WithOne(r => r.ReceiptApproval)
                .HasForeignKey<ReceiptApproval>(ra => ra.ReceiptId)
                .HasConstraintName("FK_receiptApproval_receipt")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptApproval>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.ReceiptApprovalsCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_receiptApproval_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptApproval>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.ReceiptApprovalsModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_receiptApproval_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptApproval>()
                .HasOne(ra => ra.MemberDeleted)
                .WithMany(m => m.ReceiptApprovalsDeleted)
                .HasForeignKey(ra => ra.MemberDeletedId)
                .HasConstraintName("FK_receiptApproval_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void CreatePhoto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptPhoto>().ToTable("photo", "receipt");
            modelBuilder.Entity<ReceiptPhoto>().HasKey(p => new { p.Id });
            modelBuilder.HasSequence<long>("photoSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReceiptPhoto>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ReceiptPhoto>().Property(p => p.Id).HasDefaultValueSql("nextval('receipt.\"photoSeq\"')");
            modelBuilder.Entity<ReceiptPhoto>().Property(p => p.fileExtension).HasMaxLength(256);
            modelBuilder.Entity<ReceiptPhoto>().Property(p => p.fileName).HasMaxLength(2048);

            //Foreign keys

            modelBuilder.Entity<ReceiptPhoto>()
                .HasOne(p => p.Receipt)
                .WithMany(r => r.Photos)
                .HasForeignKey(p => p.ReceiptId)
                .HasConstraintName("FK_photo_receipt")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptPhoto>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.PhotosCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_photo_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptPhoto>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.PhotosModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_photo_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptPhoto>()
                .HasOne(ra => ra.MemberDeleted)
                .WithMany(m => m.PhotosDeleted)
                .HasForeignKey(ra => ra.MemberDeletedId)
                .HasConstraintName("FK_photo_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }
        #endregion

        #region ReceiptHistory

        public void CreateReceiptHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptHistory>().ToTable("receipt", "receiptHistory");
            modelBuilder.HasSequence<long>("receiptSeq", schema: "receiptHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReceiptHistory>().Property(r => r.Id).HasDefaultValueSql("nextval('\"receiptHistory\".\"receiptSeq\"')");
            modelBuilder.Entity<ReceiptHistory>().Property(r => r.Note).HasMaxLength(2048);
            modelBuilder.Entity<ReceiptHistory>().Property(r => r.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<ReceiptHistory>().Property(r => r.RecordCreated).HasDefaultValueSql("now()");
        }

        public void CreateReceiptApprovalHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptApprovalHistory>().ToTable("approval", "receiptHistory");
            modelBuilder.HasSequence<long>("approvalSeq", schema: "receiptHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReceiptApprovalHistory>().Property(ra => ra.Id).HasDefaultValueSql("nextval('\"receiptHistory\".\"approvalSeq\"')");
            modelBuilder.Entity<ReceiptApprovalHistory>().Property(ra => ra.Note).HasMaxLength(2048);
            modelBuilder.Entity<ReceiptApprovalHistory>().Property(ra => ra.RecordCreated).HasDefaultValueSql("now()");

        }

        public void CreateCostCentreHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostCentreHistory>().ToTable("costCentre", "receiptHistory");
            modelBuilder.HasSequence<long>("costCentreSeq", schema: "receiptHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<CostCentreHistory>().Property(cc => cc.Id).HasDefaultValueSql("nextval('\"receiptHistory\".\"costCentreSeq\"')");
            modelBuilder.Entity<CostCentreHistory>().Property(cc => cc.Name).HasMaxLength(256);
            modelBuilder.Entity<CostCentreHistory>().Property(cc => cc.RecordCreated).HasDefaultValueSql("now()");
        }

        private void CreatePhotoHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoHistory>().ToTable("photo", "receiptHistory");
            modelBuilder.HasSequence<long>("photoSeq", schema: "receiptHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<PhotoHistory>().Property(p => p.Id).HasDefaultValueSql("nextval('\"receiptHistory\".\"photoSeq\"')");
            modelBuilder.Entity<PhotoHistory>().Property(p => p.RecordCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<PhotoHistory>().Property(p => p.fileExtension).HasMaxLength(256);
            modelBuilder.Entity<PhotoHistory>().Property(p => p.fileName).HasMaxLength(2048);
        }

        #endregion

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

        #region  PhotoAlbum

        private void CreateAlbums(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("albums", "photoAlbum");

                entity.HasKey(f => f.Id);

                entity.HasIndex(f => new { f.ParentAlbumId, f.Name }).IsUnique();

                entity.HasMany(f => f.ChildAlbums)
                    .WithOne(f => f.ParentAlbum)
                    .HasForeignKey(f => f.ParentAlbumId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(f => f.Photos)
                    .WithOne(p => p.AlbumLocation)
                    .HasForeignKey(p => p.AlbumLocationId);

                entity.HasMany(p => p.AlbumTags)
                    .WithOne(pt => pt.Album)
                    .HasForeignKey(pt => pt.AlbumId);
            });

        }

        private void CreateLikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("likes", "photoAlbum");
                entity.HasKey(l => l.Id);

                entity.HasOne(l => l.Member)
                    .WithMany()
                    .HasForeignKey(l => l.MemberId);

                entity.HasOne(l => l.Photo)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(l => l.PhotoId);
            });
        }

        private void CreatePhotos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photos", "photoAlbum");
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Uploader)
                    .WithMany()
                    .HasForeignKey(p => p.UploaderId);

                entity.HasOne(p => p.AlbumLocation)
                    .WithMany(f => f.Photos)
                    .HasForeignKey(p => p.AlbumLocationId);

                entity.HasMany(p => p.Likes)
                    .WithOne(l => l.Photo)
                    .HasForeignKey(l => l.PhotoId);
            });
        }

        private void CreateAlbumTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumTag>(entity =>
            {
                entity.ToTable("albumTags", "photoAlbum");
                entity.HasKey(pt => new { pt.AlbumId, pt.TagId });

                entity.HasOne(pt => pt.Album)
                    .WithMany(p => p.AlbumTags)
                    .HasForeignKey(pt => pt.AlbumId);

                entity.HasOne(pt => pt.Tag)
                    .WithMany(t => t.AlbumTags)
                    .HasForeignKey(pt => pt.TagId);
            });
        }

        private void CreateTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags", "photoAlbum");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name).HasMaxLength(255).IsRequired();

                entity.HasIndex(t => t.Name).IsUnique();

                entity.HasMany(t => t.AlbumTags)
                    .WithOne(pt => pt.Tag)
                    .HasForeignKey(pt => pt.TagId);
            });
        }

        #endregion

        #region Todo

        private void CreateToDo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().ToTable("todo", "todo");
            modelBuilder.Entity<ToDo>().HasKey(t => t.Id);
            modelBuilder.HasSequence<long>("todoSeq", "todo").StartsAt(1).IncrementsBy(1); ;
            modelBuilder.Entity<ToDo>().Property(t => t.Id).HasDefaultValueSql("nextval('todo.\"todoSeq\"')");
            modelBuilder.Entity<ToDo>().Property(t => t.Action).HasMaxLength(2048);

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.Member)
                .WithMany(m => m.ToDoOwned)
                .HasForeignKey(t => t.MemberId)
                .HasConstraintName("FK_todo_memberOwned")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToDo>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.ToDoCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_todo_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToDo>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.ToDoModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_todo_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToDo>()
                .HasOne(ra => ra.MemberDeleted)
                .WithMany(m => m.ToDoDeleted)
                .HasForeignKey(ra => ra.MemberDeletedId)
                .HasConstraintName("FK_todo_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        #endregion

        #region TodoHistory

        private void CreateToDoHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoHistory>().ToTable("todo", "todoHistory");
            modelBuilder.Entity<ToDoHistory>().HasKey(t => t.Id);
            modelBuilder.HasSequence<long>("todoSeq", "todoHistory").StartsAt(1).IncrementsBy(1); ;
            modelBuilder.Entity<ToDoHistory>().Property(t => t.Id).HasDefaultValueSql("nextval('\"todoHistory\".\"todoSeq\"')");
            modelBuilder.Entity<ToDoHistory>().Property(t => t.Action).HasMaxLength(2048);
        }

        #endregion
    }
}
