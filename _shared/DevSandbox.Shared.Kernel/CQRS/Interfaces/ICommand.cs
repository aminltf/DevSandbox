namespace DevSandbox.Shared.Kernel.CQRS.Interfaces;

public interface ICommand;

public interface ICommand<TResult> : ICommand;
