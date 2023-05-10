namespace WorkPlanner.Core.Exceptions;

public class EntityDuplicateException : Exception
{
    private const string ExceptionMessage = "A duplicate of the entity was found.";

    public EntityDuplicateException()
        : base(ExceptionMessage)
    {
    }

    public EntityDuplicateException(string message)
        : base(message)
    {
    }

    public EntityDuplicateException(string message, Exception ex)
        : base(message, ex)
    {
    }
}