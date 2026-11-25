using ActionIn.Core.Messages;
using MediatR;

namespace ActionIn.Core.Infrastructure.Mediatr;

public class MediatrHandler(IMediator mediator) : IMediatrHandler
{
    public Task PublishEvent<T>(T e, CancellationToken cancellationToken) where T : Event
        => mediator.Publish(e, cancellationToken);

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        => mediator.Send(request, cancellationToken);
}


