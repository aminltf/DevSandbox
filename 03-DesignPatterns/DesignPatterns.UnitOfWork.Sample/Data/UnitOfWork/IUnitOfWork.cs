using DesignPatterns.UnitOfWork.Sample.Data.Repositories;

namespace DesignPatterns.UnitOfWork.Sample.Data.UnitOfWork;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    void Commit();
}
