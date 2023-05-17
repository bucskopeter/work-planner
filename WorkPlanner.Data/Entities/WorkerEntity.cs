using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WorkPlanner.Data.Interfaces;
// ReSharper disable UnusedMember.Global

namespace WorkPlanner.Data.Entities;

[Index(nameof(EmailAddress), IsUnique = true)]
public class WorkerEntity : IEntity<Guid>, ISoftDeletable
{
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(320)]
    public string EmailAddress { get; set; } = null!;

    [MaxLength(15)]
    public string? PhoneNumber { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ShiftEntity> Shifts { get; set; } = null!;
}