using ActionIn.Core.Messages;
using ActionIn.Core.Messages.Commands;
using MediatR;

namespace ActionIn.Core.Mediatr;

public class MediatrHandler(IMediator mediator) : IMediatrHandler
{
    public async Task PublishEvent<T>(T e) where T : Event
    {
        await mediator.Publish(e);
    }

    public Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        => mediator.Send(request, cancellationToken);


}


