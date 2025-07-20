namespace DevSandbox.Shared.Kernel.Abstractions.Data;

public interface IContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
