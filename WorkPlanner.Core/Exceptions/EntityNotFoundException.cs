namespace WorkPlanner.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    private const string ExceptionMessage = "The entity was not found.";

    public EntityNotFoundException()
        : base(ExceptionMessage)
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception ex)
        : base(message, ex)
    {
    }
}