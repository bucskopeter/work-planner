namespace WorkPlanner.Core.Exceptions;

public class BusinessRuleViolationException : Exception
{
    private const string ExceptionMessage = "A business rule violation was attempted.";

    public BusinessRuleViolationException()
        : base(ExceptionMessage)
    {
    }

    public BusinessRuleViolationException(string message)
        : base(message)
    {
    }

    public BusinessRuleViolationException(string message, Exception ex)
        : base(message, ex)
    {
    }
}