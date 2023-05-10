using WorkPlanner.Core.Entities;

namespace WorkPlanner.Domain.Models;

public class ShiftCreateVm
{
    public DateTime Date { get; set; }

    public ShiftNumber ShiftNumber { get; set; }
}

public class ShiftVm : ShiftCreateVm
{
    public int Id { get; set; }
}