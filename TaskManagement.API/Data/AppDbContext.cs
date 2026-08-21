using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;

namespace TaskManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(2000);

            entity.Property(t => t.AssignedTo)
                .HasMaxLength(100);

            entity.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.Priority)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.IsDeleted)
                .HasDefaultValue(false);

            entity.Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(t => t.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Priority);

            entity.HasQueryFilter(t => !t.IsDeleted);

            entity.HasData(SeedData.Tasks);
        });
    }
}
