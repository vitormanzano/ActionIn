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

    public async Task<bool> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command
    {
        return await mediator.Send(command, cancellationToken);
    }

}


