using ActionIn.Core.Data;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Tests.UnitTests.Infrastructure;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task<Result> Commit(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result.Success());
    }
}


