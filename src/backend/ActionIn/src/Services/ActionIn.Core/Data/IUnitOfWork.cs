using ActionIn.Core.Results;

namespace ActionIn.Core.Data;

public interface IUnitOfWork
{
    Task<Result> Commit(CancellationToken cancellationToken = default);
}
