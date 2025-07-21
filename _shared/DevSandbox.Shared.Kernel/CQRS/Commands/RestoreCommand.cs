using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record RestoreCommand<TKey, TResult>(TKey Id) : ICommand<TResult>;
