using ActionIn.Core.DomainObjects;

namespace ActionIn.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
