namespace DevSandbox.Shared.Kernel.Exceptions;

// Exception thrown when validation fails.
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed.")
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public ValidationException(string error)
        : this(new List<string> { error })
    {
    }
}
