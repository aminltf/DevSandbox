namespace DevSandbox.Shared.Kernel.Exceptions;

// Exception thrown when an unauthorized operation is attempted.
public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Unauthorized operation.") { }

    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
}
