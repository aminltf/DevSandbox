using DevSandbox.Shared.Kernel.CQRS.Interfaces;

namespace DevSandbox.Shared.Kernel.CQRS.Queries;

public record GetByIdQuery<TKey, TResult>(TKey Id) : IQuery<TResult>;
