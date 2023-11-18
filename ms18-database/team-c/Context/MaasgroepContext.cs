using Microsoft.EntityFrameworkCore;
using Maasgroep.Database.Members;
using Maasgroep.Database.Photos;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Stock;

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
        public DbSet<Receipt> Receipt { get; set; }
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

        #region Stock

        public DbSet<Product> Product { get; set; }
        public DbSet<Stockpile> Stock { get; set; }

        #endregion

        #region StockHistory
        
        public DbSet<ProductHistory> ProductHistory { get; set; }
        public DbSet<StockpileHistory> StockHistory { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            CreateMember(modelBuilder);
            CreatePermission(modelBuilder);
            CreateMemberPermission(modelBuilder);

            CreateCostCentre(modelBuilder);
            CreateReceiptApproval(modelBuilder);
            CreateReceiptStatus(modelBuilder);
            CreateReceipt(modelBuilder);

            CreateReceiptHistory(modelBuilder);
            CreateReceiptStatusHistory(modelBuilder);
            CreateReceiptApprovalHistory(modelBuilder);
            CreateCostCentreHistory(modelBuilder);

            CreatePhoto(modelBuilder);

            CreateStockProduct(modelBuilder);
            CreateStockpile(modelBuilder);

            CreateStockProductHistory(modelBuilder);
            CreateStockpileHistory(modelBuilder);
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
            modelBuilder.Entity<Receipt>().Property(r => r.Note).HasMaxLength(2048);
            modelBuilder.Entity<Receipt>().Property(r => r.Amount).HasPrecision(18,2);

            //Foreign keys

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.CostCentre)
                .WithMany(c => c.Receipt)
                .HasForeignKey(r => r.CostCentreId)
                .HasConstraintName("FK_receipt_costCentre")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.ReceiptStatus)
                .WithMany(rs => rs.Receipt)
                .HasForeignKey(r => r.ReceiptStatusId)
                .HasConstraintName("FK_receipt_receiptStatus")
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

        #region Photo
        private void CreatePhoto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>().ToTable("photo", "photo");
            modelBuilder.Entity<Photo>().HasKey(p => new { p.Id });
            modelBuilder.HasSequence<long>("PhotoSeq", schema: "photo").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Photo>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Photo>().Property(p => p.Id).HasDefaultValueSql("nextval('photo.\"PhotoSeq\"')");

            //Foreign keys

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.ReceiptInstance)
                .WithMany(r => r.Photos)
                .HasForeignKey(p => p.Receipt)
                .HasConstraintName("FK_Photo_Receipt")
                .OnDelete(DeleteBehavior.Cascade);
        }
        #endregion

        #region Stock
        private void CreateStockProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);            
            modelBuilder.Entity<Product>().ToTable("product", "stock");
            modelBuilder.HasSequence<long>("productSeq", schema: "stock").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Product>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("nextval('stock.\"productSeq\"')");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.MemberCreated)
                .WithMany(m => m.ProductsCreated)
                .HasForeignKey(p => p.MemberCreatedId)
                .HasConstraintName("FK_stockProduct_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.MemberModified)
                .WithMany(m => m.ProductsModified)
                .HasForeignKey(p => p.MemberModifiedId)
                .HasConstraintName("FK_stockProduct_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.MemberDeleted)
                .WithMany(m => m.ProductsDeleted)
                .HasForeignKey(p => p.MemberDeletedId)
                .HasConstraintName("FK_stockProduct_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void CreateStockpile(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stockpile>().HasKey(s => s.ProductId);
            modelBuilder.Entity<Stockpile>().ToTable("stock", "stock");
            modelBuilder.Entity<Stockpile>().Property(s => s.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Stockpile>().ToTable(s => s.HasCheckConstraint("CK_stock_quantity", "\"Quantity\" >= 0"));

            modelBuilder.Entity<Stockpile>()
                .HasOne(s => s.Product)
                .WithOne(p => p.Stock)
                .HasForeignKey<Stockpile>(s => s.ProductId);

            modelBuilder.Entity<Stockpile>()
                .HasOne(s => s.MemberCreated)
                .WithMany(m => m.StocksCreated)
                .HasForeignKey(s => s.MemberCreatedId)
                .HasConstraintName("FK_stock_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Stockpile>()
                .HasOne(p => p.MemberModified)
                .WithMany(m => m.StocksModified)
                .HasForeignKey(p => p.MemberModifiedId)
                .HasConstraintName("FK_stock_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Stockpile>()
                .HasOne(p => p.MemberDeleted)
                .WithMany(m => m.StocksDeleted)
                .HasForeignKey(p => p.MemberDeletedId)
                .HasConstraintName("FK_stock_memberDeleted")
                .OnDelete(DeleteBehavior.NoAction);
        }

        #endregion

        #region Stock History

        private void CreateStockProductHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductHistory>().ToTable("product", "stockHistory");
            modelBuilder.HasSequence<long>("productSeq", schema: "stockHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<ProductHistory>().Property(r => r.Id).HasDefaultValueSql("nextval('\"stockHistory\".\"productSeq\"')");
            modelBuilder.Entity<ProductHistory>().Property(r => r.RecordCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ProductHistory>().Property(r => r.Name).HasMaxLength(2048);
        }

        private void CreateStockpileHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockpileHistory>().ToTable("stock", "stockHistory");
            modelBuilder.HasSequence<long>("stockSeq", schema: "stockHistory").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<StockpileHistory>().Property(r => r.Id).HasDefaultValueSql("nextval('\"stockHistory\".\"stockSeq\"')");
            modelBuilder.Entity<StockpileHistory>().Property(r => r.RecordCreated).HasDefaultValueSql("now()");            
        }

        #endregion
    }
}
