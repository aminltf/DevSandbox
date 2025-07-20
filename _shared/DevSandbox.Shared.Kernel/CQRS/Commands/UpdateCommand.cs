using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record UpdateCommand<TModel, TKey, TResult>(TKey Id, TModel Model) : ICommand<TResult>;
