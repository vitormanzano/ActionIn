namespace ActionIn.Core.Data;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
}
