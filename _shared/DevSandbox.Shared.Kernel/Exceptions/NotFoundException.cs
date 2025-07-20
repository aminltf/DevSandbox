namespace DevSandbox.Shared.Kernel.Exceptions;

public class NotFoundException : Exception
{
    public string? EntityName { get; }
    public object? Key { get; }

    public NotFoundException()
        : base("The requested resource was not found.") { }

    public NotFoundException(string entityName, object key)
            : base($"Entity '{entityName}' with key '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }
}
