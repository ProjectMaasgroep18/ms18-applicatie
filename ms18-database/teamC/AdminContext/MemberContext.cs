using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Members
{
    internal class MemberContext : DbContext
    {
        public DbSet<Member> Member { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<MemberPermission> MemberPermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
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
                .HasOne(m => m.UserCreated)
                .WithMany(m => m.MembersCreated)
                .HasForeignKey(m => m.UserCreatedId)
                .HasConstraintName("FK_member_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.UserModified)
                .WithMany(m => m.MembersModified)
                .HasForeignKey(m => m.UserModifiedId)
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
                .HasOne(p => p.UserCreated)
                .WithMany(m => m.PermissionsCreated)
                .HasForeignKey(p => p.UserCreatedId)
                .HasConstraintName("FK_permission_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.UserModified)
                .WithMany(m => m.PermissionsModified)
                .HasForeignKey(p => p.UserModifiedId)
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
                .HasOne(mp => mp.UserCreated)
                .WithMany(m => m.MemberPermissionsCreated)
                .HasForeignKey(mp => mp.UserCreatedId)
                .HasConstraintName("FK_memberPermission_memberCreated")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MemberPermission>()
                .HasOne(mp => mp.UserModified)
                .WithMany(m => m.MemberPermissionsModified)
                .HasForeignKey(mp => mp.UserModifiedId)
                .HasConstraintName("FK_memberPermission_memberModified")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
