using ActionIn.Core.Data;

namespace ActionIn.Authentication.Tests.UnitTests.Infrastructure;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task<bool> Commit()
    {
        return Task.FromResult(true);
    }

}


