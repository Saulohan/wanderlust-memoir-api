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
    public DbSet<Photo> Photos { get; set; }

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

        // Configure Photo entity
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Caption).HasMaxLength(500);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed destinations
        modelBuilder.Entity<Destination>().HasData(
            new Destination { Id = 1, Name = "Paris", Country = "France", IsVisited = true, Priority = Priority.High, CreatedAt = DateTime.UtcNow },
            new Destination { Id = 2, Name = "Tokyo", Country = "Japan", IsVisited = false, Priority = Priority.Medium, CreatedAt = DateTime.UtcNow },
            new Destination { Id = 3, Name = "New York", Country = "USA", IsVisited = true, Priority = Priority.High, CreatedAt = DateTime.UtcNow }
        );

        // Seed visited places
        modelBuilder.Entity<VisitedPlace>().HasData(
            new VisitedPlace 
            { 
                Id = 1, 
                Name = "Eiffel Tower", 
                Country = "France", 
                VisitDate = new DateTime(2023, 6, 15),
                Description = "Amazing experience visiting the iconic Eiffel Tower",
                Rating = 9,
                CreatedAt = DateTime.UtcNow
            },
            new VisitedPlace 
            { 
                Id = 2, 
                Name = "Central Park", 
                Country = "USA", 
                VisitDate = new DateTime(2023, 8, 20),
                Description = "Beautiful park in the heart of Manhattan",
                Rating = 8,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}