using Maasgroep.Database.Members;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database
{
	public class MemberContext : DbContext
	{
		private string _connectionString;

		public DbSet<Member> Member { get; set; }
		public DbSet<Permission> Permission { get; set; }
		public DbSet<MemberPermission> MemberPermission { get; set; }


		public MemberContext()
		{
			_connectionString = "UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true";
		}

		public MemberContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			CreateMember(modelBuilder);
			CreatePermission(modelBuilder);
			CreateMemberPermission(modelBuilder);
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
	}
}
