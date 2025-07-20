namespace DevSandbox.Shared.Kernel.Interfaces.Services;

public interface IPDFService
{
    Task<byte[]> ExportAsync<TData>(
        IEnumerable<TData> data,
        Dictionary<string, Func<TData, object?>> mappers,
        string title,
        bool landscape);
}
