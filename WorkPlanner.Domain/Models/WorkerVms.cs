namespace WorkPlanner.Domain.Models;

public class WorkerCreateVm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
}

public class WorkerVm : WorkerCreateVm
{
    public Guid Id { get; set; }
    public DateTime? Deleted { get; set; }
}