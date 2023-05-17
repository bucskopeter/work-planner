using System.ComponentModel.DataAnnotations.Schema;
using WorkPlanner.Core.Entities;
using WorkPlanner.Data.Interfaces;
// ReSharper disable UnusedMember.Global

namespace WorkPlanner.Data.Entities;

public class ShiftEntity : IEntity<int>
{
    public int Id { get; set; }

    [Column(TypeName = "date")]
    public DateTime Date { get; set; }

    [Column(TypeName = "varchar(20)")]
    public ShiftNumber ShiftNumber { get; set; }

    public Guid WorkerId { get; set; }
    public WorkerEntity Worker { get; set; } = null!;
}