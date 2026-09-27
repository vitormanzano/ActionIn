using ActionIn.Core.Messages;
using MediatR;

namespace ActionIn.Core.Mediatr;

public class MediatrHandler(IMediator mediator) : IMediatrHandler
{
    public async Task PublishEvent<T>(T e) where T : Event
    {
        await mediator.Publish(e);
    }

}


