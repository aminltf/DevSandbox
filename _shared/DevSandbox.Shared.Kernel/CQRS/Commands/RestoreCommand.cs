using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record RestoreCommand<TKey>(TKey Id) : ICommand;
