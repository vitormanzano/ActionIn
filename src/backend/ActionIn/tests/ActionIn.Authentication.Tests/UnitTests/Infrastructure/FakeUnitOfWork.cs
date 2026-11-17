using ActionIn.Core.Data;

namespace ActionIn.Authentication.Tests.UnitTests.Infrastructure;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

}


