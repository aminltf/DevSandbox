using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Commands;

public record DeleteCommand<TKey>(TKey Id) : ICommand;
