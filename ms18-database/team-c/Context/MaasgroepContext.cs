using Microsoft.EntityFrameworkCore;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Order;
using Maasgroep.Database.Context.ToDoList;

namespace Maasgroep.Database
{
    public class MaasgroepContext : DbContext
    {
        #region Members
        public DbSet<Member> Member { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<MemberPermission> MemberPermission { get; set; }
        #endregion

        #region Receipts
        public DbSet<Receipts.Receipt> Receipt { get; set; }
        public DbSet<ReceiptApproval> ReceiptApproval { get; set; }
        public DbSet<ReceiptStatus> ReceiptStatus { get; set; }
        public DbSet<CostCentre> CostCentre { get; set; }
        #endregion

        #region ReceiptHistory

        public DbSet<CostCentreHistory> CostCentreHistory { get; set; }
        public DbSet<ReceiptApprovalHistory> ReceiptApprovalHistory { get; set; }
        public DbSet<ReceiptHistory> ReceiptHistory { get; set; }
        public DbSet<ReceiptStatusHistory> ReceiptStatusHistory { get; set; }

        #endregion

        #region Photos
        public DbSet<Photo> Photo { get; set; }
        #endregion

        #region Order

        public DbSet<Product> Product { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<Line> OrderLines { get; set; }

        #endregion

        #region OrderHistory
        
        public DbSet<ProductHistory> ProductHistory { get; set; }
        public DbSet<StockHistory> StockHistory { get; set; }

        #endregion

        // TODO: Uitgecomment ivm fixture dat conflicteert met bouwen EF migration
        //private readonly Action<MaasgroepContext, ModelBuilder> _modelCustomizer;

        //public MaasgroepContext(DbContextOptions options) : base(options)
        //{
        //    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        //    optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        //    OnConfiguring(optionsBuilder);
        //}

        //public MaasgroepContext()
        //{
        //    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        //    optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        //    OnConfiguring(optionsBuilder);
        //}

        //public MaasgroepContext(DbContextOptions<MaasgroepContext> options, Action<MaasgroepContext, ModelBuilder> modelCustomizer = null) : base(options)
        //{
        //    _modelCustomizer = modelCustomizer;
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //if (_modelCustomizer is not null)
            //{
            //    _modelCustomizer(this, modelBuilder);
            //}

            CreateMember(modelBuilder);
            CreatePermission(modelBuilder);
            CreateMemberPermission(modelBuilder);

            CreateCostCentre(modelBuilder);
            CreateReceiptApproval(modelBuilder);
            CreateReceiptStatus(modelBuilder);
            CreateReceipt(modelBuilder);
            CreatePhoto(modelBuilder);

            CreateReceiptHistory(modelBuilder);
            CreateReceiptStatusHistory(modelBuilder);
            CreateReceiptApprovalHistory(modelBuilder);
            CreateCostCentreHistory(modelBuilder);
            CreatePhotoHistory(modelBuilder);


            CreateProduct(modelBuilder);
            CreateStock(modelBuilder);
            CreateProductPrice(modelBuilder);
            CreateOrderLine(modelBuilder);

            CreateProductHistory(modelBuilder);
            CreateStockHistory(modelBuilder);
            CreateProductPriceHistory(modelBuilder);
            CreateOrderLineHistory(modelBuilder);

            CreateToDo(modelBuilder);

            CreateToDoHistory(modelBuilder);
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
            modelBuilder.Entity<Receipts>().ToTable("receipt", "receipt");
            modelBuilder.Entity<Receipts>().ToTable(t => t.HasCheckConstraint("CK_receipt_amount", "\"Amount\" >= 0"));
            modelBuilder.Entity<Receipts.Receipt>().HasKey(r => new { r.Id });
            modelBuilder.HasSequence<long>("receiptSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Receipts>().Property(r => r.Id).HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");
            modelBuilder.Entity<Receipts>().Property(r => r.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Receipts.Receipt>().Property<string>(r => r.Note).HasMaxLength(2048);
            modelBuilder.Entity<Receipts.Receipt>().Property(r => r.Amount).HasPrecision(18,2);

            //Foreign keys

            modelBuilder.Entity<Receipts>()
                .HasOne(r => r.CostCentre)
                .WithMany(c => c.Receipt)
                .HasForeignKey(r => r.CostCentreId)
                .HasConstraintName("FK_receipt_costCentre")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipts>()
                .HasOne(r => r.ReceiptStatus)
                .WithMany(rs => rs.Receipt)
                .HasForeignKey(r => r.ReceiptStatusId)
                .HasConstraintName("FK_receipt_receiptStatus")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipts>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.ReceiptsCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_receipt_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipts>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.ReceiptsModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_receipt_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipts>()
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

        private void CreateReceiptStatus(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptStatus>().ToTable("status", "receipt");
            modelBuilder.Entity<ReceiptStatus>().HasKey(rs => new { rs.Id });
            modelBuilder.HasSequence<long>("statusSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.Id).HasDefaultValueSql("nextval('receipt.\"statusSeq\"')");
            modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.Name).HasMaxLength(256);
            modelBuilder.Entity<ReceiptStatus>().HasIndex(rs => rs.Name).IsUnique();

            modelBuilder.Entity<ReceiptStatus>()
                .HasOne(rs => rs.MemberCreated)
                .WithMany(m => m.ReceiptStatusesCreated)
                .HasForeignKey(rs => rs.MemberCreatedId)
                .HasConstraintName("FK_receiptStatus_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptStatus>()
                .HasOne(rs => rs.MemberModified)
                .WithMany(m => m.ReceiptStatusesModified)
                .HasForeignKey(rs => rs.MemberModifiedId)
                .HasConstraintName("FK_receiptStatus_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptStatus>()
                .HasOne(rs => rs.MemberDeleted)
                .WithMany(m => m.ReceiptStatusesDeleted)
                .HasForeignKey(rs => rs.MemberDeletedId)
                .HasConstraintName("FK_receiptStatus_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);

        }

        private void CreatePhoto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>().ToTable("photo", "receipt");
            modelBuilder.Entity<Photo>().HasKey(p => new { p.Id });
            modelBuilder.HasSequence<long>("photoSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Photo>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Photo>().Property(p => p.Id).HasDefaultValueSql("nextval('receipt.\"photoSeq\"')");

            //Foreign keys

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Receipt)
                .WithMany(r => r.Photos)
                .HasForeignKey(p => p.ReceiptId)
                .HasConstraintName("FK_photo_receipt")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Photo>()
                .HasOne(ra => ra.MemberCreated)
                .WithMany(m => m.PhotosCreated)
                .HasForeignKey(ra => ra.MemberCreatedId)
                .HasConstraintName("FK_photo_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Photo>()
                .HasOne(ra => ra.MemberModified)
                .WithMany(m => m.PhotosModified)
                .HasForeignKey(ra => ra.MemberModifiedId)
                .HasConstraintName("FK_photo_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Photo>()
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

        public void CreateReceiptStatusHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiptStatusHistory>().ToTable("status", "receiptHistory");
            modelBuilder.HasSequence<long>("statusSeq", schema: "receiptHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ReceiptStatusHistory>().Property(rs => rs.Id).HasDefaultValueSql("nextval('\"receiptHistory\".\"statusSeq\"')");
            modelBuilder.Entity<ReceiptStatusHistory>().Property(rs => rs.Name).HasMaxLength(256);
            modelBuilder.Entity<ReceiptStatusHistory>().Property(rs => rs.RecordCreated).HasDefaultValueSql("now()");
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

        #endregion


        #region Order
        private void CreateProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);            
            modelBuilder.Entity<Product>().ToTable("product", "order");
            modelBuilder.HasSequence<long>("productSeq", schema: "order").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Product>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("nextval('order.\"productSeq\"')");

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

        private void CreateProductPrice(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductPrice>().HasKey(pp => pp.ProductId);
            modelBuilder.Entity<ProductPrice>().ToTable("productPrice", "order");
            modelBuilder.Entity<ProductPrice>().ToTable(pp => pp.HasCheckConstraint("CK_orderProductPrice_price", "\"Price\" >= 0"));
            modelBuilder.Entity<ProductPrice>().Property(pp => pp.DateTimeCreated).HasDefaultValueSql("now()");

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

        private void CreateOrderLine(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Line>().HasKey(pp => pp.ProductId);
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
            throw new NotImplementedException();
        }

        private void CreateProductPriceHistory(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Todo

        private void CreateToDo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().ToTable("todo", "todo");
            modelBuilder.Entity<ToDo>().HasKey(t => t.Id);
            modelBuilder.HasSequence<long>("todoSeq", "todo").StartsAt(1).IncrementsBy(1); ;
            modelBuilder.Entity<ToDo>().Property(t => t.Id).HasDefaultValueSql("nextval('todo.\"todoSeq\"')");

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
            throw new NotImplementedException();
        }

        #endregion
    }
}
