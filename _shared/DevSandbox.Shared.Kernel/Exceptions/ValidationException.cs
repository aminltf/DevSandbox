namespace DevSandbox.Shared.Kernel.Exceptions;

// Exception thrown when validation fails.
public class ValidationException : Exception
{
    // Optional: You can add a property for validation errors
    public IDictionary<string, string[]>? Errors { get; }

    public ValidationException() : base("Validation failed.") { }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException) : base(message, innerException) { }

    public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}
