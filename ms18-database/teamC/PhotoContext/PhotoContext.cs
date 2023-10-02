using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Photos
{
    internal class PhotoContext : DbContext
    {
        public DbSet<Photo> Photo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreatePhoto(modelBuilder);
        }
        private void CreatePhoto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>().ToTable("photo", "photo");
            modelBuilder.Entity<Photo>().HasKey(p => new { p.Id });
            modelBuilder.HasSequence<long>("PhotoSeq", schema: "photo").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Photo>().Property(p => p.Created).HasDefaultValueSql("now()");
            modelBuilder.Entity<Photo>().Property(p => p.Id).HasDefaultValueSql("nextval('photo.\"PhotoSeq\"')");

            //Foreign keys

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.ReceiptInstance)
                .WithOne(r => r.Photo)
                .HasForeignKey<Photo>(p => p.Receipt)
                .HasConstraintName("FK_Photo_Receipt")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
