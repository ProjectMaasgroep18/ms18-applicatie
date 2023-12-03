using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database
{ 
	public class ReceiptContext : DbContext
	{
		private string _connectionString;

		#region Receipts
		public DbSet<Receipt> Receipt { get; set; }
		public DbSet<ReceiptApproval> ReceiptApproval { get; set; }
		public DbSet<CostCentre> CostCentre { get; set; }
		public DbSet<Photo> Photo { get; set; }
		#endregion

		#region ReceiptHistory

		public DbSet<CostCentreHistory> CostCentreHistory { get; set; }
		public DbSet<ReceiptApprovalHistory> ReceiptApprovalHistory { get; set; }
		public DbSet<ReceiptHistory> ReceiptHistory { get; set; }
		public DbSet<PhotoHistory> PhotoHistory { get; set; }

		#endregion

		public ReceiptContext(ConfigurationService configurationService)
		{
			var hoi = configurationService.GetConnectionString();
			var ditte = "";
			_connectionString = "UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true";
		}

		public ReceiptContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public ReceiptContext(DbContextOptionsBuilder optionsBuilder)
		{
			OnConfiguring(optionsBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			CreateReceipt(modelBuilder);
			CreateReceiptApproval(modelBuilder);
			CreateCostCentre(modelBuilder);
			CreatePhoto(modelBuilder);

			CreateReceiptHistory(modelBuilder);
			CreateReceiptApprovalHistory(modelBuilder);
			CreateCostCentreHistory(modelBuilder);
			CreatePhotoHistory(modelBuilder);

			CreateMemberNavigations(modelBuilder);
		}

		private void CreateMemberNavigations(ModelBuilder modelBuilder)
		{
			#region CostCentre
			modelBuilder.Entity<CostCentre>()
			.HasOne(c => c.MemberCreated)
			.WithMany(m => m.CostCentresCreated);

			modelBuilder.Entity<CostCentre>()
			.HasOne(c => c.MemberModified)
			.WithMany(m => m.CostCentresModified);

			modelBuilder.Entity<CostCentre>()
			.HasOne(c => c.MemberDeleted)
			.WithMany(m => m.CostCentresDeleted);
			#endregion

			#region Photo
			modelBuilder.Entity<Photo>()
			.HasOne(c => c.MemberCreated)
			.WithMany(m => m.PhotosCreated);

			modelBuilder.Entity<Photo>()
			.HasOne(c => c.MemberModified)
			.WithMany(m => m.PhotosModified);

			modelBuilder.Entity<Photo>()
			.HasOne(c => c.MemberDeleted)
			.WithMany(m => m.PhotosDeleted);
			#endregion

			#region ReceiptApproval
			modelBuilder.Entity<ReceiptApproval>()
			.HasOne(c => c.MemberCreated)
			.WithMany(m => m.ReceiptApprovalsCreated);

			modelBuilder.Entity<ReceiptApproval>()
			.HasOne(c => c.MemberModified)
			.WithMany(m => m.ReceiptApprovalsModified);

			modelBuilder.Entity<ReceiptApproval>()
			.HasOne(c => c.MemberDeleted)
			.WithMany(m => m.ReceiptApprovalsDeleted);
			#endregion

			#region Receipt
			modelBuilder.Entity<Receipt>()
			.HasOne(c => c.MemberCreated)
			.WithMany(m => m.ReceiptsCreated);

			modelBuilder.Entity<Receipt>()
			.HasOne(c => c.MemberModified)
			.WithMany(m => m.ReceiptsModified);

			modelBuilder.Entity<Receipt>()
			.HasOne(c => c.MemberDeleted)
			.WithMany(m => m.ReceiptsDeleted);
			#endregion

			#region Member
			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberCreated)
				.WithMany(m => m.MembersCreated);

			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberModified)
				.WithMany(m => m.MembersModified);

			modelBuilder.Entity<Member>()
				.HasOne(m => m.MemberDeleted)
				.WithMany(m => m.MembersDeleted);

			modelBuilder.Entity<Permission>()
				.HasOne(p => p.MemberCreated)
				.WithMany(m => m.PermissionsCreated);

			modelBuilder.Entity<Permission>()
				.HasOne(p => p.MemberModified)
				.WithMany(m => m.PermissionsModified);

			modelBuilder.Entity<Permission>()
				.HasOne(p => p.MemberDeleted)
				.WithMany(m => m.PermissionsDeleted);

			#endregion

		}

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
			modelBuilder.Entity<Photo>().ToTable("photo", "receipt");
			modelBuilder.Entity<Photo>().HasKey(p => new { p.Id });
			modelBuilder.HasSequence<long>("photoSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Photo>().Property(p => p.DateTimeCreated).HasDefaultValueSql("now()");
			modelBuilder.Entity<Photo>().Property(p => p.Id).HasDefaultValueSql("nextval('receipt.\"photoSeq\"')");
			modelBuilder.Entity<Photo>().Property(p => p.fileExtension).HasMaxLength(256);
			modelBuilder.Entity<Photo>().Property(p => p.fileName).HasMaxLength(2048);

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
	}
}
