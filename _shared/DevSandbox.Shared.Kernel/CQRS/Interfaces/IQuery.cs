namespace DevSandbox.Shared.Kernel.CQRS.Interfaces;

public interface IQuery;

public interface IQuery<TResult> : IQuery;
