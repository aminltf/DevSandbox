using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record DeleteCommand<TKey, TResult>(TKey Id) : ICommand<TResult>;
