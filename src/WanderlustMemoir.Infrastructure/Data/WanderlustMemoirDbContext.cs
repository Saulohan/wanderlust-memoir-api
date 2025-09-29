using Microsoft.EntityFrameworkCore;
using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Infrastructure.Data;

public class WanderlustMemoirDbContext : DbContext
{
    public WanderlustMemoirDbContext(DbContextOptions<WanderlustMemoirDbContext> options) : base(options)
    {
    }

    public DbSet<Destination> Destinations { get; set; }
    public DbSet<VisitedPlace> VisitedPlaces { get; set; }
    public DbSet<Photo> Photos { get; set; } // Manter para compatibilidade (pode ser removido depois)
    public DbSet<DestinationPhoto> DestinationPhotos { get; set; }
    public DbSet<VisitedPlacePhoto> VisitedPlacePhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Destination entity
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasMany(e => e.Photos)
                  .WithOne(p => p.Destination)
                  .HasForeignKey(p => p.DestinationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure VisitedPlace entity
        modelBuilder.Entity<VisitedPlace>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasMany(e => e.Photos)
                  .WithOne(p => p.VisitedPlace)
                  .HasForeignKey(p => p.VisitedPlaceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Photo entity (manter para compatibilidade)
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Caption).HasMaxLength(500);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImageData).IsRequired(); // BLOB field
        });

        // Configure DestinationPhoto entity
        modelBuilder.Entity<DestinationPhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Caption).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImageData).IsRequired(); // BLOB field
        });

        // Configure VisitedPlacePhoto entity
        modelBuilder.Entity<VisitedPlacePhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Caption).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImageData).IsRequired(); // BLOB field
        });
    }
}