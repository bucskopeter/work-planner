using Microsoft.EntityFrameworkCore;
using WorkPlanner.Data.Entities;
// ReSharper disable UnusedMember.Global

namespace WorkPlanner.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<ShiftEntity> Shifts { get; set; } = null!;
    public virtual DbSet<WorkerEntity> Workers { get; set; } = null!;
}