using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record CreateCommand<TModel, TResult>(TModel Model) : ICommand<TResult>;
