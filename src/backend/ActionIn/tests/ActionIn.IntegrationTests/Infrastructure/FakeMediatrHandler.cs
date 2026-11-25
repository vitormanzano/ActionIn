using ActionIn.Core.Infrastructure.Mediatr;
using ActionIn.Core.Messages;
using MediatR;

namespace ActionIn.IntegrationTests.Infrastructure;

public class FakeMediatrHandler : IMediatrHandler
{
    public Task PublishEvent<T>(T e, CancellationToken cancellationToken) where T : Event
    {
        return Task.CompletedTask;
    }
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        throw new NotSupportedException("FakeMediatrHandler does not send commands.");
    }
}


