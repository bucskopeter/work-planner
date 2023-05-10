using WorkPlanner.Data.Interfaces;

namespace WorkPlanner.Data.Entities;

public class WorkerEntity : IEntity<Guid>, ISoftDeletable
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ShiftEntity> Shifts { get; set; } = null!;
}