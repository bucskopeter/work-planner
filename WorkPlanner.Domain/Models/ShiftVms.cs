using System.ComponentModel.DataAnnotations;
using WorkPlanner.Core.Entities;
// ReSharper disable UnusedMember.Global

namespace WorkPlanner.Domain.Models;

public class ShiftCreateVm
{
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [EnumDataType(typeof(ShiftNumber))]
    public ShiftNumber ShiftNumber { get; set; }
}

public class ShiftVm : ShiftCreateVm
{
    public int Id { get; set; }
}