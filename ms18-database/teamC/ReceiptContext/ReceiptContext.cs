using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{
	internal class ReceiptContext : DbContext
	{
		public DbSet<Receipt> Receipt { get; set; }
		public DbSet<ReceiptApproval> ReceiptApproval { get; set; }
		public DbSet<ReceiptStatus> ReceiptStatus { get; set; }
		public DbSet<Store> Store { get; set; }
		public DbSet<CostCentre> CostCentre { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{			
			CreateCostCentre(modelBuilder);
			CreateStore(modelBuilder);
			CreateReceiptApproval(modelBuilder);
			CreateReceiptStatus(modelBuilder);
			CreateReceipt(modelBuilder);
		}

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
                .HasOne(cc => cc.UserCreated)
                .WithMany(m => m.CostCentresCreated)
                .HasForeignKey(cc => cc.UserCreatedId)
                .HasConstraintName("FK_costCentre_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CostCentre>()
                .HasOne(cc => cc.UserModified)
                .WithMany(m => m.CostCentresModified)
                .HasForeignKey(cc => cc.UserModifiedId)
                .HasConstraintName("FK_costCentre_memberModified")
                .OnDelete(DeleteBehavior.NoAction);
        }

		private void CreateStore(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Store>().ToTable("store", "receipt");
            modelBuilder.Entity<Store>().HasKey(s => new { s.Id });
			modelBuilder.HasSequence<long>("storeSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);			
			modelBuilder.Entity<Store>().Property(s => s.Id).HasDefaultValueSql("nextval('receipt.\"storeSeq\"')");
            modelBuilder.Entity<Store>().Property(s => s.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Store>().Property(s => s.Name).HasMaxLength(256);
            modelBuilder.Entity<Store>().HasIndex(s => s.Name).IsUnique();

            // Foreign keys

            modelBuilder.Entity<Store>()
				.HasOne(s => s.UserCreated)
				.WithMany(m => m.StoresCreated)
				.HasForeignKey(s => s.UserCreatedId)
				.HasConstraintName("FK_store_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Store>()
                .HasOne(s => s.UserModified)
                .WithMany(m => m.StoresModified)
                .HasForeignKey(s => s.UserModifiedId)
                .HasConstraintName("FK_store_memberModified")
                .OnDelete(DeleteBehavior.NoAction);
        }

		private void CreateReceipt(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Receipt>().ToTable("receipt", "receipt");
            modelBuilder.Entity<Receipt>().HasKey(r => new { r.Id });
			modelBuilder.HasSequence<long>("receiptSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Receipt>().Property(r => r.Id).HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");
            modelBuilder.Entity<Receipt>().Property(r => r.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Receipt>().Property(r => r.Note).HasMaxLength(2048);
            modelBuilder.Entity<Receipt>().Property(r => r.Amount).HasPrecision(2);

            //Foreign keys

            modelBuilder.Entity<Receipt>()
				.HasOne(r => r.Store)
				.WithMany(s => s.Receipt)
				.HasForeignKey(r => r.StoreId)
				.HasConstraintName("FK_receipt_store")
				.OnDelete(DeleteBehavior.NoAction);

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
                .HasOne(ra => ra.UserCreated)
                .WithMany(m => m.ReceiptsCreated)
                .HasForeignKey(ra => ra.UserCreatedId)
                .HasConstraintName("FK_receipt_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Receipt>()
                .HasOne(ra => ra.UserModified)
                .WithMany(m => m.ReceiptsModified)
                .HasForeignKey(ra => ra.UserModifiedId)
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
                .HasOne(ra => ra.UserCreated)
                .WithMany(m => m.ReceiptApprovalsCreated)
                .HasForeignKey(ra => ra.UserCreatedId)
                .HasConstraintName("FK_receiptApproval_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptApproval>()
                .HasOne(ra => ra.UserModified)
                .WithMany(m => m.ReceiptApprovalsModified)
                .HasForeignKey(ra => ra.UserModifiedId)
                .HasConstraintName("FK_receiptApproval_memberModified")
                .OnDelete(DeleteBehavior.NoAction);
        }

		private void CreateReceiptStatus(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ReceiptStatus>().ToTable("status", "receipt");
            modelBuilder.Entity<ReceiptStatus>().HasKey(rs => new { rs.Id });
			modelBuilder.HasSequence<long>("receiptStatusSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.Id).HasDefaultValueSql("nextval('receipt.\"receiptStatusSeq\"')");
            modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ReceiptStatus>().Property(rs => rs.Name).HasMaxLength(256);
            modelBuilder.Entity<ReceiptStatus>().HasIndex(rs => rs.Name).IsUnique();

            modelBuilder.Entity<ReceiptStatus>()
                .HasOne(rs => rs.UserCreated)
                .WithMany(m => m.ReceiptStatusesCreated)
                .HasForeignKey(rs => rs.UserCreatedId)
                .HasConstraintName("FK_receiptStatus_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceiptStatus>()
                .HasOne(rs => rs.UserModified)
                .WithMany(m => m.ReceiptStatusesModified)
                .HasForeignKey(rs => rs.UserModifiedId)
                .HasConstraintName("FK_receiptStatus_memberModified")
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
