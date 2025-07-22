using DesignPatterns.UnitOfWork.Sample.Entities;

namespace DesignPatterns.UnitOfWork.Sample.Data.Repositories;

public interface ICustomerRepository
{
    void Add(Customer customer);
    Customer? GetById(Guid id);
    IEnumerable<Customer> GetAll();
    void Remove(Guid id);
}
