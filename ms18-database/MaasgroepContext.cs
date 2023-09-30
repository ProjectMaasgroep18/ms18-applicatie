using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database
{
	public class MaasgroepContext : DbContext
	{
		public DbSet<Receipt> Receipt { get; set; }
		public DbSet<ReceiptApproval> ReceiptApproval { get; set; }
		public DbSet<ReceiptStatus> ReceiptStatus { get; set; }
		public DbSet<Photo> Photo { get; set; }
		public DbSet<Store> Store { get; set; }
		public DbSet<CostCentre> CostCentre { get; set; }
		public DbSet<MaasgroepMember> MaasgroepMember { get; set; }

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
			CreatePhoto(modelBuilder);
			CreateMaasgroepMember(modelBuilder);
		}


        private void CreatePhoto(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Photo>().ToTable("photo", "photo");
			modelBuilder.HasSequence<long>("PhotoSeq", schema: "photo").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Photo>().Property(p => p.Created).HasDefaultValueSql("now()");
			modelBuilder.Entity<Photo>().Property(p => p.Id).HasDefaultValueSql("nextval('photo.\"PhotoSeq\"')");

            //FK
            modelBuilder.Entity<Photo>()
				.HasOne(p => p.ReceiptInstance)
				.WithOne(r => r.Photo)
				.HasForeignKey<Photo>(p => p.Receipt)
				.HasConstraintName("FK_Photo_Receipt")
				.OnDelete(DeleteBehavior.NoAction);
		}

		private void CreateCostCentre(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CostCentre>().ToTable("costCentre", "receipt");
			modelBuilder.HasSequence<long>("costCentreSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);			
			modelBuilder.Entity<CostCentre>().Property(c => c.Id).HasDefaultValueSql("nextval('receipt.\"costCentreSeq\"')");
            modelBuilder.Entity<CostCentre>().Property(c => c.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<CostCentre>().HasIndex(c => c.Name).IsUnique();

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
			modelBuilder.HasSequence<long>("storeSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);			
			modelBuilder.Entity<Store>().Property(s => s.Id).HasDefaultValueSql("nextval('receipt.\"storeSeq\"')");
            modelBuilder.Entity<Store>().Property(s => s.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<Store>().HasIndex(s => s.Name).IsUnique();

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
			modelBuilder.HasSequence<long>("receiptSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<Receipt>().Property(r => r.Id).HasDefaultValueSql("nextval('receipt.\"receiptSeq\"')");
            modelBuilder.Entity<Receipt>().Property(r => r.DateTimeCreated).HasDefaultValueSql("now()");

            //FK
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
            modelBuilder.Entity<ReceiptApproval>().Property(ra => ra.DateTimeCreated).HasDefaultValueSql("now()");

            //FK
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
			modelBuilder.HasSequence<long>("receiptStatusSeq", schema: "receipt").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<ReceiptStatus>().Property(r => r.Id).HasDefaultValueSql("nextval('receipt.\"receiptStatusSeq\"')");
            modelBuilder.Entity<ReceiptStatus>().Property(r => r.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<ReceiptStatus>().HasIndex(r => r.Name).IsUnique();

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

        private void CreateMaasgroepMember(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MaasgroepMember>().ToTable("member", "admin");
			modelBuilder.HasSequence<long>("memberSeq", schema: "admin").StartsAt(1).IncrementsBy(1);
			modelBuilder.Entity<MaasgroepMember>().Property(m => m.Id).HasDefaultValueSql("nextval('admin.\"memberSeq\"')");
            modelBuilder.Entity<MaasgroepMember>().Property(m => m.DateTimeCreated).HasDefaultValueSql("now()");
            modelBuilder.Entity<MaasgroepMember>().HasIndex(m => m.Name).IsUnique();

			modelBuilder.Entity<MaasgroepMember>()
				.HasOne(m => m.UserCreated)
				.WithMany(m => m.MembersCreated)
				.HasForeignKey(m => m.UserCreatedId)
				.HasConstraintName("FK_member_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaasgroepMember>()
                .HasOne(m => m.UserModified)
                .WithMany(m => m.MembersModified)
                .HasForeignKey(m => m.UserModifiedId)
                .HasConstraintName("FK_member_memberModified")
				.OnDelete(DeleteBehavior.NoAction);
        }
    }
}
