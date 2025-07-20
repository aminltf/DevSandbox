namespace DevSandbox.Shared.Kernel.Exceptions;

// Exception thrown when a conflict occurs (e.g., duplicate data, concurrency issues).
public class ConflictException : Exception
{
    public ConflictException() : base("A conflict has occurred.") { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }
}
