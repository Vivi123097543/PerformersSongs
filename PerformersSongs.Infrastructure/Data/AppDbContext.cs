using Microsoft.EntityFrameworkCore;
using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Performer> Performers => Set<Performer>();

    public DbSet<Song> Songs => Set<Song>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Performer>()
            .HasMany(p => p.Songs)
            .WithOne(s => s.Performer)
            .HasForeignKey(s => s.PerformerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Performer>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<Song>()
            .Property(s => s.Title)
            .IsRequired();

        modelBuilder.Entity<Song>()
            .Property(s => s.ChartPosition)
            .IsRequired();
    }
}
