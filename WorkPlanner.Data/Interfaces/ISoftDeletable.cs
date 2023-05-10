namespace WorkPlanner.Data.Interfaces;

public interface ISoftDeletable
{
    DateTime? Deleted { get; set; }
}