using WorkPlanner.Core.Entities;
using WorkPlanner.Data.Interfaces;

namespace WorkPlanner.Data.Entities;

public class ShiftEntity : IEntity<int>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public ShiftNumber ShiftNumber { get; set; }

    public Guid WorkerId { get; set; }
    public WorkerEntity Worker { get; set; } = null!;
}