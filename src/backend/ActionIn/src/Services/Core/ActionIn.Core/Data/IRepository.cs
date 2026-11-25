using ActionIn.Core.DomainObjects;

namespace ActionIn.Core.Data;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
