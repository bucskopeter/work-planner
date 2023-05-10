using Microsoft.EntityFrameworkCore;
using WorkPlanner.Data.Entities;

namespace WorkPlanner.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerEntity>().Property(i => i.DateOfBirth).HasColumnType("date");
        modelBuilder.Entity<ShiftEntity>().Property(i => i.Date).HasColumnType("date");

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<ShiftEntity> Shifts { get; set; } = null!;
    public virtual DbSet<WorkerEntity> Workers { get; set; } = null!;
}