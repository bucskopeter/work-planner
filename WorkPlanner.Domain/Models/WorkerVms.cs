using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace WorkPlanner.Domain.Models;

public class WorkerCreateVm
{
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; } = null!;

    [StringLength(50, MinimumLength = 3)]
    public string LastName { get; set; } = null!;

    [EmailAddress] 
    public string EmailAddress { get; set; } = null!;

    [Phone]
    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }
}

public class WorkerVm : WorkerCreateVm
{
    public Guid Id { get; set; }
    public DateTime? Deleted { get; set; }
}