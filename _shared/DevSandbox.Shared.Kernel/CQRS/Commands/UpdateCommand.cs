using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record UpdateCommand<TModel, TResult>(TModel Model) : ICommand<TResult>;
