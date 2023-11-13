using Microsoft.EntityFrameworkCore;
using Maasgroep.Database.Members;
using Maasgroep.Database.Photos;
using Maasgroep.Database.Receipts;

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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5454;Database=Maasgroep;Pooling=true");
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
                .WithOne(r => r.Photo)
                .HasForeignKey<Photo>(p => p.Receipt)
                .HasConstraintName("FK_Photo_Receipt")
                .OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
